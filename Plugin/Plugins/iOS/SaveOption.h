@interface SaveOption : NSObject {
@public
    BOOL saveToLibrary;
    NSString* fileName;
}

-(id)init;
-(id)init:(NSString*)jsonString;
@end
