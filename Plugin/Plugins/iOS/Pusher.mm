#import "Pusher.h"

@implementation PushBridge

+(void)SendToken:(NSString*)token;
{
    UnitySendMessage("_Pusher", "", [token cStringUsingEncoding:NSUTF8StringEncoding]);
}

+(void)RequestPushToken
{
    if ([[[UIDevice currentDevice] systemVersion] floatValue] < 8.0)
    {
        [[UIApplication sharedApplication] registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound | UIRemoteNotificationTypeAlert)];
    }
    else
    {
        [[UIApplication sharedApplication] registerUserNotificationSettings:[UIUserNotificationSettings settingsForTypes:(UIUserNotificationTypeSound | UIUserNotificationTypeAlert | UIUserNotificationTypeBadge) categories:nil]];
        
        [[UIApplication sharedApplication] registerForRemoteNotifications];
    }
}

@end


extern "C"
{
    //Dummy. Will remove it
    void _Bridge_RequestPushToken(){}
    void _Pusher_Initialize() {
        [PushBridge RequestPushToken];
    }
}
