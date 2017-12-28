#import <Foundation/Foundation.h>
#import "Json.h"
#import "SaveOption.h"

@implementation SaveOption
-(id)init {
    self = [super init];
    self->saveToLibrary = false;
    self->fileName = nil;
    return self;
}

-(id)init:(NSString*)jsonString {
    self = [self init];
    NSDictionary* json = [Json Parse:jsonString];
    if(json != nil) {
        self->saveToLibrary = [[json objectForKey:@"SaveToLibrary"] boolValue];
        self->fileName = [json objectForKey:@"FileName"];
    }
    return self;
}
@end
