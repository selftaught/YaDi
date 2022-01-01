
namespace YADI.Enums
{
    public enum BinaryType : uint
    {
        SCS_32BIT_BINARY = 0, /* A 32-bit Windows-based application */
        SCS_64BIT_BINARY = 6, /* A 64-bit Windows-based application. */
        SCS_DOS_BINARY = 1,   /* An MS-DOS – based application */
        SCS_OS216_BINARY = 5, /* A 16-bit OS/2-based application */
        SCS_PIF_BINARY = 3,   /* A PIF file that executes an MS-DOS – based application */
        SCS_POSIX_BINARY = 4, /* A POSIX – based application */
        SCS_WOW_BINARY = 2    /* A 16-bit Windows-based application */
    }
}
