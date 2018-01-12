#import <Foundation/Foundation.h>
#import "NyanPath.h"

@implementation NyanPath

+(NSString*)GetCachesPath:(NSString*)innerPath {
    innerPath = [@"RenkoL/Caches/" stringByAppendingString:innerPath];
    return [NyanPath GetDocumentPath:innerPath];
}

+(NSString*)GetTempPath:(NSString*)innerPath {
    innerPath = [@"RenkoL/Temp/" stringByAppendingString:innerPath];
    return [NyanPath GetDocumentPath:innerPath];
}

+(NSString*)GetDocumentPath:(NSString *)innerPath {
    NSString* basePath = [[NSFileManager defaultManager]URLsForDirectory:NSDocumentDirectory inDomains:NSUserDomainMask][0].absoluteString;
    if([basePath containsString:@"file://"])
        basePath = [basePath substringFromIndex:7];
    if(innerPath == nil)
        return basePath;
    return [basePath stringByAppendingString:innerPath];
}

@end
