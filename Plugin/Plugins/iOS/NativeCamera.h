#import <Foundation/Foundation.h>
#import <MobileCoreServices/MobileCoreServices.h>
@class SaveOption;
@class CropOption;
@class VideoOption;

@interface NativeCamera : NSObject <UINavigationControllerDelegate, UIImagePickerControllerDelegate>{
    @public
    NSString* messageObject;
    SaveOption* lastSaveOption;
    CropOption* lastCropOption;
    VideoOption* lastVideoOption;
    UIImagePickerController* cameraController;
    
    BOOL isTakingPhoto;
}

-(id)init:(NSString*)objectName;

-(void)TakePhoto:(SaveOption*)saveOption :(CropOption*)cropOption;
-(void)TakeVideo:(SaveOption*)saveOption :(VideoOption*)videoOption;

-(void)PhotoCallback:(NSString*)savedPath;
-(void)VideoCallback:(NSString*)savedPath;

@end
