# Lists the released and testing opponents in listing.xml (with path
# to listing.xml prepended)
from __future__ import print_function

import os
import sys
import xml.etree.ElementTree as ET

if len(sys.argv) > 1:
    filename = sys.argv[1]
    directory_name = os.path.dirname(filename)
else:    
    directory_name = os.path.dirname(sys.argv[0])
    filename = os.path.join(directory_name, "listing.xml")

tree = ET.parse(filename)

for opp in tree.getroot().find('individuals'):
    if 'status' not in opp.attrib or opp.attrib['status'] in ("testing", "incomplete", "offline"):
        print(os.path.join(directory_name, opp.text))
