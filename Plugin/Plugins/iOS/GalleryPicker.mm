#import <MobileCoreServices/UTCoreTypes.h>
#import "GalleryPicker.h"
#import "Utils.h"
#import "CropOption.h"
#import "Vector2.h"

static NSString* RESULT_ERROR = @"error";
static NSString* RESULT_CANCEL = @"cancel";
static GalleryPicker* Instance;

@implementation GalleryPicker
-(id)init:(NSString*)objectName {
    self = [super init];
    self->messageObject = objectName;
    return self;
}

-(void)SetOptions:(NSString*)savePath :(CropOption*)cropOption {
    self->savePath = [Utils GetDocumentPath:savePath];
    if(self->isPickingImage)
        self->savePath = [self->savePath stringByAppendingString:@".jpg"];
    else
        self->savePath = [self->savePath stringByAppendingString:@".mov"];
    
    self->lastCropOption = cropOption;
}

-(void)PickImage:(NSString*)savePath :(CropOption*)cropOption {
    self->isPickingImage = true;
    [self SetOptions:savePath :cropOption];
    [self OpenLibrary];
}

-(void)PickVideo:(NSString*)savePath {
    self->isPickingImage = false;
    [self SetOptions:savePath :nil];
    [self OpenLibrary];
}

-(void)OpenLibrary {
    UIImagePickerController* controller = [[UIImagePickerController alloc] init];
    controller.delegate = self;
    controller.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    
    if(self->isPickingImage) {
        controller.allowsEditing = (self->lastCropOption->isCropping);
    }
    else {
        controller.allowsEditing = false;
        controller.mediaTypes = [[NSArray alloc] initWithObjects:(NSString *)kUTTypeVideo, (NSString *)kUTTypeMovie, nil];
    }
    
    [UnityGetGLViewController() presentViewController:controller animated:YES completion:nil];
}

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary<NSString *, id> *)info {
    if(self->isPickingImage) {
        UIImage* img = [UIImage imageWithData:UIImageJPEGRepresentation([info valueForKey:UIImagePickerControllerEditedImage], 1.0f)];
        if(img == nil)
            img = [UIImage imageWithData:UIImageJPEGRepresentation([info valueForKey:UIImagePickerControllerOriginalImage], 1.0f)];
        
        if(img != nil) {
            NSData *imgData = UIImageJPEGRepresentation( img, 1.0f );
            BOOL success = [imgData writeToFile:self->savePath atomically:FALSE];
            if(success) {
                [picker dismissViewControllerAnimated:YES completion:^() {
                    [self ImageCallback:self->savePath];
                }];
            }
            else {
                [picker dismissViewControllerAnimated:YES completion:^() {
                    [self ImageCallback:RESULT_ERROR];
                }];
            }
        }
        else {
            [self ImageCallback:RESULT_ERROR];
        }
    }
    else {
        // NEEDS IMPLEMENTATION!
        [self VideoCallback:RESULT_ERROR];
    }
}

-(void)imagePickerControllerDidCancel:(UIImagePickerController *)picker {
    [picker dismissViewControllerAnimated:YES completion:^() {
        if(self->isPickingImage)
            [self ImageCallback:RESULT_CANCEL];
        else
            [self VideoCallback:RESULT_CANCEL];
    }];
}

-(void)ImageCallback:(NSString*)result {
    UnitySendMessage([self->messageObject cStringUsingEncoding:NSUTF8StringEncoding], "OnImagePicked", [result cStringUsingEncoding:NSUTF8StringEncoding]);
}

-(void)VideoCallback:(NSString*)result {
    UnitySendMessage([self->messageObject cStringUsingEncoding:NSUTF8StringEncoding], "OnVideoPicked", [result cStringUsingEncoding:NSUTF8StringEncoding]);
}
@end

extern "C"
{
    void _GalleryPicker_Initialize(const char* objectName) {
        Instance = [[GalleryPicker alloc] init:[NSString stringWithUTF8String:objectName]];
    }
    
    void _GalleryPicker_Destroy() {
        Instance = nil;
    }
    
    void _GalleryPicker_PickImage(const char* targetPath, const char* cropOption) {
        NSString* path = [NSString stringWithUTF8String:targetPath];
        CropOption* crop = [[CropOption alloc] init:[NSString stringWithUTF8String:cropOption]];
        [Instance PickImage:path :crop];
    }
    
    void _GalleryPicker_PickVideo(const char* targetPath) {
        NSString* path = [NSString stringWithUTF8String:targetPath];
        [Instance PickVideo:path];
    }
}
