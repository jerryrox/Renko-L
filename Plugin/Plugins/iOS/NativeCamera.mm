#import "NativeCamera.h"
#import "UnityAppController.h"
#import "SaveOption.h"
#import "CropOption.h"
#import "VideoOption.h"
#import "Utils.h"
#import "Permissions.h"

static NSString* RESULT_ERROR = @"error";
static NSString* RESULT_CANCEL = @"cancel";

@implementation NativeCamera
-(id)init:(NSString*)objectName {
    self = [super init];
    self->messageObject = objectName;
    return self;
}

-(void)SetOptions:(SaveOption*)saveOption :(CropOption*)cropOption :(VideoOption*)videoOption {
    saveOption->fileName = [Utils GetDocumentPath:saveOption->fileName];
    if(self->isTakingPhoto)
        saveOption->fileName = [saveOption->fileName stringByAppendingString:@".jpg"];
    else
        saveOption->fileName = [saveOption->fileName stringByAppendingString:@".mov"];
    self->lastSaveOption = saveOption;
    
    self->lastCropOption = cropOption;
    self->lastVideoOption = videoOption;
}

-(void)TakePhoto:(SaveOption *)saveOption :(CropOption *)cropOption {
    self->isTakingPhoto = true;
    [self SetOptions:saveOption :cropOption :nil];
    [self OpenCamera];
}
-(void)TakeVideo:(SaveOption *)saveOption :(VideoOption *)videoOption {
    self->isTakingPhoto = false;
    [self SetOptions:saveOption :nil :videoOption];
    
    [self OpenCamera];
}
-(void)OpenCamera {
    // Camera must be available for use...
    if ([UIImagePickerController isSourceTypeAvailable: UIImagePickerControllerSourceTypeCamera]) {
        self->cameraController = [[UIImagePickerController alloc] init];
        self->cameraController.delegate = self;
        self->cameraController.sourceType = UIImagePickerControllerSourceTypeCamera;
        
        if(self->isTakingPhoto) {
            self->cameraController.allowsEditing = self->lastCropOption->isCropping;
            self->cameraController.mediaTypes = [UIImagePickerController availableMediaTypesForSourceType:UIImagePickerControllerSourceTypeCamera];
            self->cameraController.mediaTypes = [NSArray arrayWithObjects:(NSString *)kUTTypeImage, nil];
        }
        else {
            self->cameraController.allowsEditing = (self->lastVideoOption->maxDuration > 0);
            self->cameraController.mediaTypes = [UIImagePickerController availableMediaTypesForSourceType:UIImagePickerControllerSourceTypeCamera];
            self->cameraController.mediaTypes = [NSArray arrayWithObjects:(NSString *)kUTTypeMovie, nil];
        }
        
        self->cameraController.cameraCaptureMode = (self->isTakingPhoto ? UIImagePickerControllerCameraCaptureModePhoto : UIImagePickerControllerCameraCaptureModeVideo);
        
        // Mode-specifiec finalization
        if(self->isTakingPhoto) {
            
        }
        else {
            if(self->lastVideoOption->maxDuration > 0)
                self->cameraController.videoMaximumDuration = self->lastVideoOption->maxDuration;
        }
        [UnityGetGLViewController() presentViewController:self->cameraController animated:YES completion:nil];
    }
    else {
        [Utils Alert:@"Error" :@"Couldn't open the camera."];
        
        if(self->isTakingPhoto)
            [self PhotoCallback:RESULT_ERROR];
        else
            [self VideoCallback:RESULT_ERROR];
    }
}

//////////////////////////////////////////////////////////////
// Camera callbacks
//////////////////////////////////////////////////////////////
-(void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary<NSString *,id> *)info {
    [picker dismissViewControllerAnimated:YES completion:nil];
    
    if(self->isTakingPhoto) {
        UIImage* takenImage = (UIImage *) [info objectForKey:UIImagePickerControllerEditedImage];
        if(takenImage == nil)
            takenImage = (UIImage *) [info objectForKey: UIImagePickerControllerOriginalImage];
        
        if(self->lastSaveOption->saveToLibrary)
            UIImageWriteToSavedPhotosAlbum(takenImage, nil, nil, nil);
        
        NSData* jpegImage = UIImageJPEGRepresentation(takenImage, 1.0);
        BOOL success = [jpegImage writeToFile:self->lastSaveOption->fileName atomically:YES];
        if(success)
            [self PhotoCallback:self->lastSaveOption->fileName];
        else
            [self PhotoCallback:RESULT_ERROR];
    }
    else {
        NSURL* url = [info objectForKey:UIImagePickerControllerMediaURL];
        NSString *moviePath = [[info objectForKey: UIImagePickerControllerMediaURL] path];
        
        if(self->lastSaveOption->saveToLibrary) {
            if (UIVideoAtPathIsCompatibleWithSavedPhotosAlbum(moviePath))
                UISaveVideoAtPathToSavedPhotosAlbum(moviePath, nil, nil, nil);
            else
                [self VideoCallback:RESULT_ERROR];
        }
        
        NSData *videoData = [NSData dataWithContentsOfURL:url];
        BOOL success = [videoData writeToFile:self->lastSaveOption->fileName atomically:NO];
        if(success)
            [self VideoCallback:self->lastSaveOption->fileName];
        else
            [self VideoCallback:RESULT_ERROR];
    }
}

-(void)imagePickerControllerDidCancel:(UIImagePickerController *)picker {
    [picker dismissViewControllerAnimated:YES completion:nil];
    
    if(self->isTakingPhoto)
        [self PhotoCallback:RESULT_CANCEL];
    else
        [self VideoCallback:RESULT_CANCEL];
}

//////////////////////////////////////////////////////////////
// Callback
//////////////////////////////////////////////////////////////
-(void)PhotoCallback:(NSString*) result {
    UnitySendMessage([self->messageObject cStringUsingEncoding:NSUTF8StringEncoding], "OnPhotoCallback", [result cStringUsingEncoding:NSUTF8StringEncoding]);
}

-(void)VideoCallback:(NSString*) result {
    UnitySendMessage([self->messageObject cStringUsingEncoding:NSUTF8StringEncoding], "OnVideoCallback", [result cStringUsingEncoding:NSUTF8StringEncoding]);
}
@end

extern "C"
{
    static NativeCamera* Instance;
    
    void _NativeCamera_Initialize(const char* objectName) {
        [Permissions RequestForCamera];
        Instance = [[NativeCamera alloc] init:[NSString stringWithUTF8String:objectName]];
    }
    
    void _NativeCamera_Destroy() {
        Instance = nil;
    }
    
    void _NativeCamera_TakePhoto(const char* saveOption, const char* cropOption) {
        SaveOption* so = [[SaveOption alloc] init:[NSString stringWithUTF8String:saveOption]];
        CropOption* co = [[CropOption alloc] init:[NSString stringWithUTF8String:cropOption]];
        [Instance TakePhoto:so :co];
    }
    void _NativeCamera_TakeVideo(const char* saveOption, const char* videoOption) {
        SaveOption* so = [[SaveOption alloc] init:[NSString stringWithUTF8String:saveOption]];
        VideoOption* vo = [[VideoOption alloc] init:[NSString stringWithUTF8String:videoOption]];
        [Instance TakeVideo:so :vo];
    }
}
