#import "CameraPlugin.h"
#import "UnityAppController.h"

static CameraPlugin* cInstance;

const NSString* RESULT_ERROR = "error";
const NSString* RESULT_CANCEL = "cancel";
static BOOL bSaveToPhotoLibrary;

static BOOL bCaptureImage;

@interface NativeCamera

@property(nonatomic, strong) NSString*

@end


@implementation NativeCamera


-(id)init
{
    id plugin = [super init];
    
    cInstance = plugin;
    
    return plugin;
}
-(void)dealloc
{
    cInstance = nil;
    cCameraController = nil;
}

//////////////////////////////////////////////////////////////
// Open camera & Capture
//////////////////////////////////////////////////////////////
-(void)TakePicture:(BOOL)saveToPhotoLibrary :(NSString*)customPath
{
    [cInstance SetParameters:saveToPhotoLibrary :customPath];
    
    [cInstance OpenCamera:UIImagePickerControllerCameraCaptureModePhoto :0];
}
-(void)TakeVideo:(BOOL)saveToPhotoLibrary :(NSString*)customPath :(int)maxDuration
{
    [cInstance SetParameters:saveToPhotoLibrary :customPath];
    
    [cInstance OpenCamera:UIImagePickerControllerCameraCaptureModeVideo :maxDuration];
}
-(void)OpenCamera:(UIImagePickerControllerCameraCaptureMode)captureMode :(int)maxDuration
{
    //Determine which capture type
    bCaptureImage = (captureMode == UIImagePickerControllerCameraCaptureModePhoto);
    
    //Check if camera is avaliable
    if ([UIImagePickerController isSourceTypeAvailable: UIImagePickerControllerSourceTypeCamera])
    {
        //Init camera controller
        cInstance.cCameraController = [[UIImagePickerController alloc] init];
        
        //Set delegate for callback
        cInstance.cCameraController.delegate = cInstance;
        
        //Set source type
        cInstance.cCameraController.sourceType = UIImagePickerControllerSourceTypeCamera;
        
        //Set editable
        cInstance.cCameraController.allowsEditing = (!bCaptureImage && maxDuration > 0);
        
        //If photo
        if(bCaptureImage)
        {
            cInstance.cCameraController.mediaTypes = [UIImagePickerController availableMediaTypesForSourceType:UIImagePickerControllerSourceTypeCamera];
            cInstance.cCameraController.mediaTypes = [NSArray arrayWithObjects:(NSString *)kUTTypeImage, nil];
        }
        else
        {
            cInstance.cCameraController.mediaTypes = [UIImagePickerController availableMediaTypesForSourceType:UIImagePickerControllerSourceTypeCamera];
            cInstance.cCameraController.mediaTypes = [NSArray arrayWithObjects:(NSString *)kUTTypeMovie, nil];
        }
        
        
        //Photo or video capture
        cInstance.cCameraController.cameraCaptureMode = captureMode;
        
        //If video capturing and duration is set
        if(!bCaptureImage && maxDuration > 0)
        {
            cCameraController.videoMaximumDuration = maxDuration;
        }
        
        //Show the camera controller
        [UnityGetGLViewController() presentViewController:cCameraController animated: YES completion: nil];
    }
    else
    {
        //Show message and quit
        UIAlertView *view = [[UIAlertView alloc]
                             initWithTitle:@"Error"
                             message:@"Cannot open camera."
                             delegate:nil
                             cancelButtonTitle:nil
                             otherButtonTitles:@"OK", nil];
        
        [view show];
        
        //Callback for fail
        if(bCaptureImage)
            [cInstance _CP_TakePhotoResult:@"error"];
        else
            [cInstance _CP_TakeVideoResult:@"error"];
    }
}

-(void)SetParameters:(BOOL)saveToPhotoLibrary :(NSString*)customPath
{
    bSaveToPhotoLibrary = saveToPhotoLibrary;
    szCustomPath = customPath;
    
    if(bSaveToPhotoLibrary)
    {
        szCustomPath = nil;
    }
}

//////////////////////////////////////////////////////////////
// Camera callbacks
//////////////////////////////////////////////////////////////
-(void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary<NSString *,id> *)info
{
    [picker dismissViewControllerAnimated:YES completion:nil];
    
    UIImage* takenImage;
    
    //If image
    if(bCaptureImage)
    {
        //Get the image taken
        takenImage = (UIImage *) [info objectForKey:UIImagePickerControllerEditedImage];
        
        if(!takenImage)
        {
            takenImage = (UIImage *) [info objectForKey: UIImagePickerControllerOriginalImage];
        }
        
        //If save to library
        if(bSaveToPhotoLibrary)
        {
            UIImageWriteToSavedPhotosAlbum(takenImage, nil, nil, nil);
            [cInstance _CP_TakePhotoResult:@"Photo library"];
            
        }
        else
        {
            NSData* jpegImage = UIImageJPEGRepresentation(takenImage, 1.0);
            [jpegImage writeToFile:szCustomPath atomically:YES];
            [cInstance _CP_TakePhotoResult:szCustomPath];
        }
    }
    else
    {
        NSURL* url = [info objectForKey:UIImagePickerControllerMediaURL];
        NSString *moviePath = [[info objectForKey: UIImagePickerControllerMediaURL] path];
        
        NSLog(@"Movie path: %@", moviePath);
        
        //If save to library
        if(bSaveToPhotoLibrary)
        {
            if (UIVideoAtPathIsCompatibleWithSavedPhotosAlbum(moviePath))
            {
                UISaveVideoAtPathToSavedPhotosAlbum(moviePath, nil, nil, nil);
                [cInstance _CP_TakeVideoResult:@"library"];
            }
            else
                [cInstance _CP_TakeVideoResult:@"error"];
        }
        else
        {
            NSData *videoData = [NSData dataWithContentsOfURL:url];
            NSLog(@"VideoData length:  %ld", (unsigned long)videoData.length
                  );
            BOOL success = [videoData writeToFile:szCustomPath atomically:NO];
            if(success)
                [cInstance _CP_TakeVideoResult:szCustomPath];
            else
                [cInstance _CP_TakeVideoResult:@"error"];
        }
    }
}

-(void)imagePickerControllerDidCancel:(UIImagePickerController *)picker
{
    [picker dismissViewControllerAnimated:YES completion:nil];
    
    if(bSaveToPhotoLibrary)
        [self _CP_TakePhotoResult:@"cancel"];
    else
        [self _CP_TakeVideoResult:@"cancel"];
}


//////////////////////////////////////////////////////////////
// Callback
//////////////////////////////////////////////////////////////
-(void)_CP_TakePhotoResult:(NSString*)savedPath
{
    NSLog(@"_CP_TakePhotoResult: %@", savedPath);
    const char* path = savedPath.UTF8String;
    UnitySendMessage("_sc_camPlugin", "CP_PhotoCallback", path);
}
-(void)_CP_TakeVideoResult:(NSString*)savedPath
{
    NSLog(@"_CP_TakeVideoResult: %@", savedPath);
    const char* path = savedPath.UTF8String;
    UnitySendMessage("_sc_camPlugin", "CP_VideoCallback", path);
}

@end

extern "C"
{
    void* _CP_Initialize()
    {
        return [[CameraPlugin alloc] init];
    }
    void _CP_Dispose(void* instance)
    {
        CameraPlugin* plugin = (CameraPlugin*)instance;
        [plugin release];
    }
    
    void _CP_TakePhoto(BOOL saveToPhotoLibrary, const char* customPath)
    {
        cInstance.szCustomPath = [NSString stringWithCString:customPath encoding:NSUTF8StringEncoding];
        [cInstance TakePicture:saveToPhotoLibrary :cInstance.szCustomPath];
    }
    void _CP_TakeVideo(BOOL saveToPhotoLibrary, int maxDuration, const char* customPath)
    {
        //if(maxDuration > 600)
        //    maxDuration = 10;
        NSLog(@"maxDuration: %d", maxDuration);
        cInstance.szCustomPath = [NSString stringWithUTF8String:customPath];
        [cInstance TakeVideo:saveToPhotoLibrary :cInstance.szCustomPath :maxDuration];
    }
}
