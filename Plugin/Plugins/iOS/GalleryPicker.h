#import <Foundation/Foundation.h>
@class CropOption;

@interface GalleryPicker : NSObject <UINavigationControllerDelegate, UIImagePickerControllerDelegate> {
    @public
    NSString* messageObject;
    NSString* savePath;
    CropOption* lastCropOption;
    BOOL isPickingImage;
}

-(id)init:(NSString*)objectName;

-(void)PickImage:(NSString*)savePath :(CropOption*)cropOption;
-(void)PickVideo:(NSString*)savePath;

-(void)ImageCallback:(NSString*)result;
-(void)VideoCallback:(NSString*)result;

@end
