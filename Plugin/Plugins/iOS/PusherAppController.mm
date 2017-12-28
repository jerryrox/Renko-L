#import <UIKit/UIKit.h>
#import "UnityAppController.h"
#import "Pusher.h"
#import "ExtensionNSData.h"

@interface PusherAppController : UnityAppController
{
}
@end

@implementation PusherAppController

///////////////////////////////////////////
//         AppDelegate Functions         //
///////////////////////////////////////////
-(BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    
    [PushBridge RequestPushToken];
    if(launchOptions != nil)
    {
        launchOptions = [launchOptions valueForKey:@"UIApplicationLaunchOptionsRemoteNotificationKey"];
        if(launchOptions != nil) {
        }
    }
    [[UIApplication sharedApplication] setApplicationIconBadgeNumber: 0];
    return true;
}

-(void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(nonnull NSData *)deviceToken
{
    [super application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
    
    NSString* token = [deviceToken hexadecimalString];
    [PushBridge SendToken:token];
}

-(void)application:(UIApplication *)application didFailToRegisterForRemoteNotificationsWithError:(NSError *)error
{
    [super application:application didFailToRegisterForRemoteNotificationsWithError:error];
}

-(void)application:(UIApplication*)application didReceiveRemoteNotification:(nonnull NSDictionary *)userInfo
{
    [super application:application didReceiveRemoteNotification:userInfo];
}

-(void)applicationDidBecomeActive:(UIApplication *)application
{
    [super applicationDidBecomeActive:application];
    [[UIApplication sharedApplication] setApplicationIconBadgeNumber: 0];
}

- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation
{
    return [super application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
}

@end


IMPL_APP_CONTROLLER_SUBCLASS(PusherAppController)

