@interface NyanPath : NSObject

+(NSString*)GetCachesPath:(NSString*)innerPath;
+(NSString*)GetTempPath:(NSString*)innerPath;
+(NSString*)GetDocumentPath:(NSString*)innerPath;

@end
