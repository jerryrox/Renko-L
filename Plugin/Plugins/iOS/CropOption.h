@class Vector2;

@interface CropOption : NSObject {
    @public
    Vector2* ratio;
    Vector2* size;
    BOOL isCropping;
}

-(id)init;
-(id)init:(NSString*)jsonString;
@end
