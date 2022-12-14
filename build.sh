#!/bin/sh

# Move to src dir
APP_PATH=`echo $0 | awk '{split($0,patharr,"/"); idx=1; while(patharr[idx+1] != "") { if (patharr[idx] != "/") {printf("%s/", patharr[idx]); idx++ }} }'`
APP_PATH=`cd "$APP_PATH"; pwd` 
cd "$APP_PATH"

Usage()
{
	echo "Usage: $0 [-h] [-r] [csproj]"
	echo 
	echo "Build the mono version of a SwinGame using the supplied"
	echo "csproj file, or by locating it within the current directory." 
	echo
	echo "Options:"
	echo " -h = Show usage" 
	echo " -r = Use Release build"
	exit 1
}

BUILD="DEBUG"

while getopts hr o
do
	case "$o" in
	h)  Usage;;
	r)  BUILD="RELEASE";;
	esac
done

shift $((${OPTIND}-1))

if [ $BUILD = "DEBUG" ]
then
	BUILD_OPT="/p:Configuration=Debug"
	BIN_DIR="./bin/Debug"
else
	BUILD_OPT="/p:Configuration=Release"
	BIN_DIR="./bin/Release"
fi

if [ -d ${RESOURCE_DIR} ]
then
	echo "Removing old build files..."
	rm -rf ${BIN_DIR}
fi

if [ -f /System/Library/Frameworks/Cocoa.framework/Cocoa ]
then
	echo ""
	echo "Building Mac version"
	echo "===================="
	echo " Running xbuild $1 ${BUILD_OPT}"
	
	#xbuild $1 ${BUILD_OPT} > out.log
	 gmcs /out:bin/Debug/GameProject.exe *.cs ./Properties/*.cs  /target:winexe /reference:System.dll /reference:System.Drawing.dll /reference:lib/SGSDK.NET.dll > out.log 2> out.log
	if [ $? != 0 ]; then echo "Error during compilation"; cat out.log; exit 1; fi
	
	echo "  Copying library files..."
	cp ./lib/SGSDK.NET.dll ${BIN_DIR}
	if [ $? != 0 ]; then echo "Error copying library"; exit 1; fi
	cp ./lib/*.dylib ${BIN_DIR}
	if [ $? != 0 ]; then echo "Error copying library"; exit 1; fi

	FRAMEWORK_DIR=./bin/Frameworks
	
	if [ -d $FRAMEWORK_DIR ]
	then
		rm -rf "$FRAMEWORK_DIR"
	fi
	mkdir -p $FRAMEWORK_DIR
	cp -R ./lib/*.framework $FRAMEWORK_DIR
	
	RESOURCE_DIR=./bin
else
  echo ""
	echo "Building Linux version"
	echo "======================"
	echo " Running xbuild $1 ${BUILD_OPT}"
	
	xbuild $1 ${BUILD_OPT} > out.log
	if [ $? != 0 ]; then echo "Error with xbuild"; cat out.log; exit 1; fi

	echo "  Copying library files to ${BIN_DIR}..."
	cp ./lib/SGSDK.NET.dll  ${BIN_DIR}
	if [ $? != 0 ]; then echo "Error copying library"; exit 1; fi

	cp ./lib/*.so  ${BIN_DIR}
	if [ $? != 0 ]; then echo "Error copying library"; exit 1; fi
		
	RESOURCE_DIR=${BIN_DIR}
fi

SOURCE_RESOURCE=./res

echo "  Copying resources to ${RESOURCE_DIR}..."

if [ -d ${RESOURCE_DIR} ]
then
  echo "   Skip creating directory"
else
  mkdir ${RESOURCE_DIR}
  if [ $? != 0 ]; then echo "Error creating resource directory"; exit 1; fi
fi
	
mkdir ${RESOURCE_DIR}/fonts
if [ $? != 0 ]; then echo "Error creating fonts directory"; exit 1; fi
	
mkdir ${RESOURCE_DIR}/images
if [ $? != 0 ]; then echo "Error creating images directory"; exit 1; fi
	
mkdir ${RESOURCE_DIR}/sounds
if [ $? != 0 ]; then echo "Error creating sounds directory"; exit 1; fi
	
mkdir ${RESOURCE_DIR}/maps
if [ $? != 0 ]; then echo "Error creating maps directory"; exit 1; fi

find ${SOURCE_RESOURCE} -maxdepth 1 -type f -exec cp {} ${RESOURCE_DIR} \;
if [ $? != 0 ]; then echo "Error copying resources"; exit 1; fi

find ${SOURCE_RESOURCE}/fonts -maxdepth 1 -type f -exec cp {} ${RESOURCE_DIR}/fonts \;
if [ $? != 0 ]; then echo "Error copying fonts"; exit 1; fi

find ${SOURCE_RESOURCE}/images -maxdepth 1 -type f -exec cp {} ${RESOURCE_DIR}/images \;
if [ $? != 0 ]; then echo "Error copying images"; exit 1; fi

find ${SOURCE_RESOURCE}/sounds -maxdepth 1 -type f -exec cp {} ${RESOURCE_DIR}/sounds \;
if [ $? != 0 ]; then echo "Error copying sounds"; exit 1; fi

find ${SOURCE_RESOURCE}/maps -maxdepth 1 -type f -exec cp {} ${RESOURCE_DIR}/maps \;
if [ $? != 0 ]; then echo "Error copying maps"; exit 1; fi

echo "Done."
