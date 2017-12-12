#import <Foundation/Foundation.h>


@interface Netko : NSObject



@end

@implementation Netko



@end


extern "C"
{
    void Netko_RemoveCache()
    {
        [[NSURLCache sharedURLCache] removeAllCachedResponses];
    }
}
