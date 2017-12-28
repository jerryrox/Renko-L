@interface Vector2 : NSObject {
@public
    float x;
    float y;
}

-(id)init;
-(id)init:(float)x :(float)y;
-(id)init:(NSString*)jsonString;
@end
