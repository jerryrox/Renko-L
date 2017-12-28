#import <Foundation/Foundation.h>
#import "VideoOption.h"
#import "Json.h"

@implementation VideoOption
-(id)init {
    self = [super init];
    self->maxDuration = 0;
    return self;
}

-(id)init:(NSString*)jsonString {
    self = [self init];
    NSDictionary* json = [Json Parse:jsonString];
    if(json != nil) {
        self->maxDuration = [[json objectForKey:@"MaxDuration"] intValue];
    }
    return self;
}
@end
