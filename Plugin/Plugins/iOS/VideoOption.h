@interface VideoOption : NSObject {
    @public
    int maxDuration;
}

-(id)init;
-(id)init:(NSString*)jsonString;
@end
