#import <UIKit/UIKit.h>
#import "UnityAppController.h"
#import "ExtensionNSData.h"

@interface RenkoAppController : UnityAppController
{
}
@end

@implementation RenkoAppController

///////////////////////////////////////////
//         AppDelegate Functions         //
///////////////////////////////////////////
-(BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions {
    
    return [super application:application didFinishLaunchingWithOptions:launchOptions];
}

-(void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(nonnull NSData *)deviceToken {
    [super application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
}

-(void)application:(UIApplication *)application didFailToRegisterForRemoteNotificationsWithError:(NSError *)error {
    [super application:application didFailToRegisterForRemoteNotificationsWithError:error];
}

-(void)application:(UIApplication*)application didReceiveRemoteNotification:(nonnull NSDictionary *)userInfo {
    [super application:application didReceiveRemoteNotification:userInfo];
}

-(void)applicationDidBecomeActive:(UIApplication *)application {
    [super applicationDidBecomeActive:application];
}

- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation {
    return [super application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
}

@end


IMPL_APP_CONTROLLER_SUBCLASS(RenkoAppController)

