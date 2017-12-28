#import <Foundation/Foundation.h>
#import "Json.h"

@implementation Json
+(NSDictionary*)Parse:(NSString *)value {
    NSError* error = nil;
    NSDictionary* json = [NSJSONSerialization JSONObjectWithData:[value dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error: &error];
    return json;
}
@end
