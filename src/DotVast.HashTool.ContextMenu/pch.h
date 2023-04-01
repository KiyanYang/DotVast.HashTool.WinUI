#pragma once

#define WIN32_LEAN_AND_MEAN
#define NOMCX
#define NOHELP
#define NOCOMM
#include <windows.h>

#include <sstream>
#include <vector>

#include <shlwapi.h>
#include <shobjidl_core.h>

#include <winrt/base.h>

#pragma comment(lib, "shlwapi.lib")
#pragma comment(lib, "windowsapp")
