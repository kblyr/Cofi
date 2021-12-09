using System.Reflection;

namespace Cofi.Identity.EntityFrameworkCore;

public static class AssemblyMarker
{
    public static Assembly Assembly { get; } = typeof(AssemblyMarker).Assembly;
}