#import <Foundation/Foundation.h>
#import "Json.h"
#import "CropOption.h"
#import "Vector2.h"

@implementation CropOption
-(id)init {
    self = [super init];
    self->ratio = [[Vector2 alloc] init];
    self->size = [[Vector2 alloc] init];
    self->isCropping = false;
    return self;
}

-(id)init:(NSString*)jsonString {
    self = [self init];
    NSDictionary* json = [Json Parse:jsonString];
    if(json != nil) {
        NSDictionary* ratio = [json objectForKey:@"Ratio"];
        NSDictionary* size = [json objectForKey:@"Size"];
        self->ratio = [[Vector2 alloc] init:[[ratio objectForKey:@"x"] floatValue] :[[ratio objectForKey:@"y"] floatValue]];
        self->size = [[Vector2 alloc] init:[[size objectForKey:@"x"] floatValue] :[[size objectForKey:@"y"] floatValue]];
        self->isCropping = [[json objectForKey:@"IsCropping"] boolValue];
    }
    return self;
}
@end
