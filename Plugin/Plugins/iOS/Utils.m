#import "Utils.h"

@implementation Utils
+(NSString*)GetDocumentPath:(NSString *)innerPath {
    NSString* basePath = [[NSFileManager defaultManager]URLsForDirectory:NSDocumentDirectory inDomains:NSUserDomainMask][0].absoluteString;
    if([basePath containsString:@"file://"])
        basePath = [basePath substringFromIndex:7];
    if(innerPath == nil)
        return basePath;
    return [basePath stringByAppendingString:innerPath];
}

+(void)Alert:(NSString *)title :(NSString *)message {
    [Utils Alert:title :message :nil :@"Confirm"];
}

+(void)Alert:(NSString*)title :(NSString*)message :(NSString*)cancelMessage :(NSString*)otherMessage {
    UIAlertView* view = [[UIAlertView alloc] initWithTitle:title message:message delegate:nil cancelButtonTitle:cancelMessage otherButtonTitles:otherMessage, nil];
    [view show];
}
@end
