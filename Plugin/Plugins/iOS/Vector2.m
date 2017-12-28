#import <Foundation/Foundation.h>
#import "Json.h"
#import "Vector2.h"

@implementation Vector2
-(id)init {
    self = [super init];
    self->x = self->y = 0;
    return self;
}

-(id)init:(float)x :(float)y {
    self = [super init];
    self->x = x;
    self->y = y;
    return self;
}

-(id)init:(NSString*)jsonString {
    self = [self init];
    NSDictionary* json = [Json Parse:jsonString];
    if(json != nil) {
        self->x = [[json objectForKey:@"x"] floatValue];
        self->y = [[json objectForKey:@"y"] floatValue];
    }
    return self;
}
@end
