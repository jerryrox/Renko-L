#import <AVFoundation/AVFoundation.h>
#import <Foundation/Foundation.h>
#import <AVKit/AVKit.h>

@implementation Permissions

+(void)RequestForCamera
{
    if ([AVCaptureDevice respondsToSelector:@selector(requestAccessForMediaType: completionHandler:)])
    {
        [AVCaptureDevice requestAccessForMediaType:AVMediaTypeVideo completionHandler:^(BOOL granted) {
            // Will get here on both iOS 7 & 8 even though camera permissions weren't required
            // until iOS 8. So for iOS 7 permission will always be granted.
            if (granted) {
                // Permission has been granted. Use dispatch_async for any UI updating
                // code because this block may be executed in a thread.
                dispatch_async(dispatch_get_main_queue(), ^{
                });
            } else {
                // Permission has been denied.
            }
        }];
    }
    else
    {
        // We are on iOS <= 6. Just do what we need to do.
    }
}

@end

