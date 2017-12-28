//
//  NSData.m
//  Unity-iPhone
//
//  Created by softchips on 1/4/16.
//
//

#import "ExtensionNSData.h"

@implementation NSData (ExtensionNSData)

-(NSString*)hexadecimalString {
    const unsigned char *dataBuffer = (const unsigned char *)[self bytes];
    if (!dataBuffer)
        return [NSString string];
    
    NSUInteger          dataLength  = [self length];
    NSMutableString     *hexString  = [NSMutableString stringWithCapacity:(dataLength * 2)];
    for (int i = 0; i < dataLength; ++i)
        [hexString appendString:[NSString stringWithFormat:@"%02lx", (unsigned long)dataBuffer[i]]];
    return [NSString stringWithString:hexString];
}

@end
