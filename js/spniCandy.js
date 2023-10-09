// ui elements
$titleCandy = [$("#left-title-candy"), $("#right-title-candy")];

// global variables
var candyCatalog = [];
var candyTotalWeight = 0;
var candyTransform = ["translateX(-50%)", "translateX(50%)"];

/**
 * Randomly selects two candy images for the title screen. If no candy image
 * options are currently loaded, it will load them from XML first.
 * @returns {void}
 */
function selectTitleCandy () {
    console.log("Selecting title candy...");

    if (!candyCatalog || candyCatalog.length <= 0) {
        loadCandyXML().then(() => randomizeTitleCandy());
    } else {
        randomizeTitleCandy();
    }
}

/**
 * Loads the candy image catalog from XML.
 * @returns {Promise<jQuery>}
 */
function loadCandyXML () {
    return fetchXML("opponents/candy.xml").then($xml => {
        candyCatalog = [];
        candyTotalWeight = 0;

        $xml.find("candy").each((i, item) => {
            let $item = $(item);
            let candy = {
                uid: $item.attr("uid"),
                uids: $item.attr("uids")?.split(","),
                base: $item.attr("base"),
                src: $item.attr("src"),
                scale: parseFloat($item.attr("scale")),
                weight: parseFloat($item.attr("weight")),
                slot: parseInt($item.attr("slot")),
            };

            if (candy.slot) {
                // double the weight of any candy that has a slot restriction
                // otherwise it would be half as likely than normal to appear 
                candy.weight = candy.weight ? candy.weight * 2 : 2.0;
            }

            if (candy.uid && candy.uids && !candy.uids.includes(candy.uid)) {
                candy.uids.push(candy.uid);
            } else if (candy.uid && !candy.uids) {
                candy.uids = [candy.uid];
            }

            candyTotalWeight += candy.weight ? candy.weight : 1.0;
            candyCatalog.push(candy);
        });
    });
}

/**
 * Randomly selects and places two unique title candy images.
 * @returns {void}
 */
function randomizeTitleCandy () {
    let candySpaces = 2;
    let currentCatalog = candyCatalog.slice();

    for (let i = 0; i < candySpaces && currentCatalog.length > 0; i++) {
        // filter the catalog to include only candy that accepts this slot index
        let slotWeight = 0;
        let slotCatalog = currentCatalog.filter((candy) => {
            if (!candy.slot || candy.slot === i) {
                slotWeight += candy.weight ? candy.weight : 1.0;
                return true;
            }
            return false;
        });

        // generate a random weight based on the current weight of the catalog
        let random = getRandomNumber(0, slotWeight);

        // find the candy matching the given random weight
        let track = 0;
        let choice = slotCatalog.find((candy, index) => {
            track += candy.weight ? candy.weight : 1.0;
            if (track > random) {
                return candy;
            }
        });

        // if a no candy was selected then skip this slot and move on
        if (!choice) continue;
        placeTitleCandy(i, choice);

        // if more candy is to be selected, filter the catalog to avoid duplicates
        if (i < candySpaces - 1) {
            currentCatalog = currentCatalog.filter((candy) => {
                if (choice.uids.some((uid) => candy.uids.includes(uid))) {
                    return false;
                } else if (choice.legacy && choice.base === candy.base) {
                    return false;
                }
                return true;
            });
        }
    }
}

/**
 * Places a candy in the requested title candy slot.
 * @param {number} index The index of the title candy slot to place the candy in.
 * @param {object} candy The candy to place.
 * @returns {void}
 */
function placeTitleCandy (index, candy) {
    // resolve scaling for legacy event candies
    if (candy.legacy && !candy.scale) {
        let characterID = getCharacterForCostume(candy.path);
        candy.scale = (loadedOpponents.find(c => c.id == characterID).scale / 100) || 1;
    }

    // place the selected candy
    let scale = candy.scale ? candy.scale : 1.0;
    let base = "opponents/" + (candy.base ? candy.base : candy.uid) + "/";

    $titleCandy[index].attr("src", base + candy.src);
    $titleCandy[index].css("transform", "scale(" + scale + ") " + candyTransform[index]);
}

/**
 * Overrides the candy catalog with legacy event candy paths.
 * @param {Set} eventSet A set of legacy image paths from an event.
 * @returns {void}
 */
function useEventTitleCandy (eventSet) {
    let newCatalog = [];
    let newTotalWeight = 0;

    eventSet.forEach((item) => {
        let last = item.lastIndexOf("/");
        let base = item.slice(0, last);
        let src = item.slice(last + 1);

        let candy = {
            base,
            src,
            path: item,
            legacy: true,
        };

        newTotalWeight += 1.0;
        newCatalog.push(candy);
    });

    if (newCatalog.length > 0) {
        candyTotalWeight = newTotalWeight;
        candyCatalog = newCatalog;
    }
}

/**
 * Resolves a costume path to a character ID. Currently only used to support 
 * legacy event candy paths.
 * @param {string} costumePath The costume path to resolve to a character.
 * @returns {string} The resolved character ID, if found.
 */
function getCharacterForCostume(costumePath) {
    const match = costumePath.split("/");
    
    if (match[0] != "reskins") {
        return match[0];
    }
    const opponent = loadedOpponents.find(opp => {
        // opponent has a costume that fits
        return opp.alternate_costumes.findIndex(costume => costume.folder.endsWith(match[1]+ "/")) != -1;
    });
    if (opponent == undefined) {
        console.log(`Couldn't find opponent for costume "${costumePath}". The costume may be offline.`);
        return "";
    }
    return opponent.id;
}
