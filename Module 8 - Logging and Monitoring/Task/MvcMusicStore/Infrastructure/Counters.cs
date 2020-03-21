using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using PerformanceCounterHelper;

namespace MvcMusicStore.Infrastructure
{
    [PerformanceCounterCategory("MvcMusicStore", PerformanceCounterCategoryType.MultiInstance, "MvsMusicStore")]
    public enum Counters
    {
        [PerformanceCounter("Go to home count", "Go to home", System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        GoToHome,

        [PerformanceCounter("Successful login", "Login", PerformanceCounterType.NumberOfItems32)]
        Login,
        
        [PerformanceCounter("Successful log out", "Log out", PerformanceCounterType.NumberOfItems32)]
        LogOff
    }
}