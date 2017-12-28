#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface PushBridge : NSObject

+(void)SendToken:(NSString*)token;
+(void)RequestPushToken;

@end
