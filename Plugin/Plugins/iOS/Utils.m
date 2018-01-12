#import "Utils.h"

@implementation Utils

+(void)Alert:(NSString *)title :(NSString *)message {
    [Utils Alert:title :message :nil :@"Confirm"];
}

+(void)Alert:(NSString*)title :(NSString*)message :(NSString*)cancelMessage :(NSString*)otherMessage {
    UIAlertView* view = [[UIAlertView alloc] initWithTitle:title message:message delegate:nil cancelButtonTitle:cancelMessage otherButtonTitles:otherMessage, nil];
    [view show];
}
@end
