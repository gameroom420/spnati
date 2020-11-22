#!/bin/bash

mkdir -p .public/opponents .public/img
cp -r css fonts js player index.html version-info.xml backgrounds.xml .public

# Copy non-recursively to exclude the backgrounds folder.
cp img/* .public/img

TIMESTAMP="`date +%s`000"
sed -r "s/(var VERSION_TAG).*/\1 = '${VERSION}';/g; s/(var VERSION_TIMESTAMP).*/\1 = ${TIMESTAMP}/g" js/version.js > .public/js/version.js
cp opponents/listing.xml .public/opponents
cp opponents/general_collectibles.xml .public/opponents

# Copy online background images and set initial background to display during
# loading.
python3 deploy-scripts/copy_backgrounds.py .public/

# tar may be the easiest way to copy an arbitrary
# list of files, keeping the directory structure.
# Include *.js and *.css to accommodate Monika.
find `python opponents/list_opponents.py` -regextype egrep -iregex '.*\.(png|gif|jpe?g|xml|js|css|woff2?)' | tar -cT - | tar -C .public -x

# Copy alternate costume files for deployment.
python3 deploy-scripts/copy_alternate_costumes.py .public/ ./ all

# Rename JS and core game CSS for cache-busting purposes.
python3 deploy-scripts/cache_bust.py .public/

python3 opponents/fill_linecount_metadata.py .public/opponents
python3 deploy-scripts/fill_update_timestamps.py .public/opponents opponents
python opponents/gzip_dialogue.py .public/opponents/*/behaviour.xml
python opponents/analyze_image_space.py .public/opponents
