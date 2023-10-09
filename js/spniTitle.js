/********************************************************************************
 This file contains the variables and functions that form the title and setup screens
 of the game. The parsing functions for the player.xml file, the clothing organization
 functions, and human player initialization.
 ********************************************************************************/

/**********************************************************************
 *****                   Title Screen UI Elements                 *****
 **********************************************************************/

$titlePanels = [$("#title-panel-1"), $("#title-panel-2")];
$nameField = $("#player-name-field");
$warningContainer = $('#initial-warning');
$titleContainer = $('#main-title-container');
$sizeBlocks = { male: $('#male-size-container'), female: $('#female-size-container') };
$clothingTable = $("#title-clothing-table");
$warningLabel = $("#title-warning-label");

var $gameLoadLabel = $(".game-load-label");
var $gameLoadProgress = $(".game-load-progress");

/**********************************************************************
 *****                    Title Screen Variables                  *****
 **********************************************************************/

/* maybe move this data to an external file if the hardcoded stuff changes often enough */
var playerTagOptions = {
    'hair_color': {
        values: [
            { value: 'black_hair' }, { value: 'white_hair' },
            { value: 'brunette' }, { value: 'ginger' }, { value: 'blonde' },
            { value: 'green_hair' },
            { value: 'blue_hair' },
            { value: 'purple_hair' },
            { value: 'pink_hair' },
        ],
    },
    'eye_color': {
        values: [
            { value: 'brown_eyes' }, { value: 'dark_eyes' },
            { value: 'pale_eyes' }, { value: 'red_eyes' },
            { value: 'amber_eyes' }, { value: 'green_eyes' },
            { value: 'blue_eyes' }, { value: 'violet_eyes' },
            { value: 'pink_eyes' },
        ],
    },
    'skin_color': {
        type: 'range',
        values: [
            { value: 'pale-skinned', from: 0, to: 25 },
            { value: 'fair-skinned', from: 25, to: 50 },
            { value: 'olive-skinned', from: 50, to: 75 },
            { value: 'dark-skinned', from: 75, to: 100 },
        ],
    },
    'hair_length': {
        values: [
            { value: 'bald', text: 'Bald - No Hair'},
            { value: 'short_hair', text: 'Short Hair - Does Not Pass Jawline'},
            { value: 'medium_hair', text: 'Medium Hair - Reaches Between Jawline and Shoulders'},
            { value: 'long_hair', text: 'Long Hair - Reaches Beyond Shoulders'},
            { value: 'very_long_hair', text: 'Very Long Hair - Reaches the Thighs or Beyond'},
        ],
    },
    'physical_build': {
        values: [
            { value: 'skinny' },
            { value: 'chubby' },
            { value: 'curvy', gender: 'female' },
            { value: 'athletic' },
            { value: 'muscular' },
        ],
    },
    'height': {
        values: [
            { value: 'tall' },
            { value: 'average' },
            { value: 'short' },
        ],
    },
    'pubic_hair_style': {
        values: [
            { value: 'shaved' },
            { value: 'trimmed' },
            { value: 'hairy' },
        ],
    },
    'circumcision': {
        gender: 'male',
        values: [
            { value: 'circumcised' },
            { value: 'uncircumcised' }
        ],
    },
    'sexual_orientation': {
        values: [
            { value: 'straight' },
            { value: 'bi-curious' },
            { value: 'bisexual' },
            { value: 'reverse_bi-curious', gender: 'male', text: 'Male-leaning bi-curious ' },
            { value: 'reverse_bi-curious', gender: 'female', text: 'Female-leaning bi-curious' },
            { value: 'gay', gender: 'male' },
            { value: 'lesbian', gender: 'female' },
        ]
    }
};
var playerTagSelections = {};

/* Order matters here. */
var DEFAULT_CLOTHING_OPTIONS = [
    new PlayerClothing('hat', 'hat', EXTRA_ARTICLE, 'head', "player/male/hat.png", false, "hat", "all", null),
    new PlayerClothing('headphones', 'headphones', EXTRA_ARTICLE, 'head', "player/male/headphones.png", true, "headphones", "all", null),

    /****/

    new PlayerClothing('jacket', 'jacket', MINOR_ARTICLE, UPPER_ARTICLE, "player/male/jacket.png", false, "jacketA", "male", null),
    new PlayerClothing('shirt', 'shirt', MAJOR_ARTICLE, UPPER_ARTICLE, "player/male/shirt.png", false, "shirtA", "male", null),
    new PlayerClothing('t-shirt', 'shirt', MAJOR_ARTICLE, UPPER_ARTICLE, "player/male/tshirt.png", false, "tshirt", "male", null),
    new PlayerClothing('undershirt', 'shirt', IMPORTANT_ARTICLE, UPPER_ARTICLE, "player/male/undershirt.png", false, "undershirt", "male", null),

    new PlayerClothing('jacket', 'jacket', MINOR_ARTICLE, UPPER_ARTICLE, "player/female/jacket.png", false, "jacketB", "female", null),
    new PlayerClothing('shirt', 'shirt', MAJOR_ARTICLE, UPPER_ARTICLE, "player/female/shirt.png", false, "shirtB", "female", null),
    new PlayerClothing('tank top', 'shirt', MAJOR_ARTICLE, UPPER_ARTICLE, "player/female/tanktop.png", false, "tanktop", "female", null),
    new PlayerClothing('bra', 'bra', IMPORTANT_ARTICLE, UPPER_ARTICLE, "player/female/bra.png", false, "bra", "female", null),

    /****/

    new PlayerClothing('glasses', 'glasses', EXTRA_ARTICLE, 'head', "player/male/glasses.png", true, "glasses", "all", null),
    new PlayerClothing('belt', 'belt', EXTRA_ARTICLE, 'waist', "player/male/belt.png", false, "belt", "all", null),

    /****/

    new PlayerClothing('pants', 'pants', MAJOR_ARTICLE, LOWER_ARTICLE, "player/male/pants.png", true, "pantsA", "male", null),
    new PlayerClothing('shorts', 'shorts', MAJOR_ARTICLE, LOWER_ARTICLE, "player/male/shorts.png", true, "shortsA", "male", null),
    new PlayerClothing('kilt', 'skirt', MAJOR_ARTICLE, LOWER_ARTICLE, "player/male/kilt.png", false, "kilt", "male", null),
    new PlayerClothing('boxers', 'underwear', IMPORTANT_ARTICLE, LOWER_ARTICLE, "player/male/boxers.png", true, "boxers", "male", null),

    new PlayerClothing('pants', 'pants', MAJOR_ARTICLE, LOWER_ARTICLE, "player/female/pants.png", true, "pantsB", "female", null),
    new PlayerClothing('shorts', 'shorts', MAJOR_ARTICLE, LOWER_ARTICLE, "player/female/shorts.png", true, "shortsB", "female", null),
    new PlayerClothing('skirt', 'skirt', MAJOR_ARTICLE, LOWER_ARTICLE, "player/female/skirt.png", false, "skirt", "female", null),
    new PlayerClothing('panties', 'underwear', IMPORTANT_ARTICLE, LOWER_ARTICLE, "player/female/panties.png", true, "panties", "female", null),

    /****/

    new PlayerClothing('necklace', 'jewelry', EXTRA_ARTICLE, 'neck', "player/male/necklace.png", false, "necklace", "all", null),
    new PlayerClothing('gloves', 'gloves', EXTRA_ARTICLE, 'hands', "player/male/gloves.png", true, "gloves", "all", null),

    /****/

    new PlayerClothing('tie', 'tie', EXTRA_ARTICLE, 'neck', "player/male/tie.png", false, "tie", "male", null),

    new PlayerClothing('bracelet', 'jewelry', EXTRA_ARTICLE, 'arms', "player/female/bracelet.png", false, "bracelet", "female", null),

    /****/

    new PlayerClothing('socks', 'socks', MINOR_ARTICLE, 'feet', "player/male/socks.png", true, "socksA", "male", null),
    new PlayerClothing('shoes', 'shoes', EXTRA_ARTICLE, 'feet', "player/male/shoes.png", true, "shoesA", "male", null),
    new PlayerClothing('boots', 'shoes', EXTRA_ARTICLE, 'feet', "player/male/boots.png", true, "boots", "male", null),

    new PlayerClothing('stockings', 'socks', MINOR_ARTICLE, 'legs', "player/female/stockings.png", true, "stockings", "female", null),
    new PlayerClothing('socks', 'socks', MINOR_ARTICLE, 'feet', "player/female/socks.png", true, "socksB", "female", null),
    new PlayerClothing('shoes', 'shoes', EXTRA_ARTICLE, 'feet', "player/female/shoes.png", true, "shoesB", "female", null),
];

/**
 * @type {Object<string, PlayerClothing>}
 */
var PLAYER_CLOTHING_OPTIONS = {};
DEFAULT_CLOTHING_OPTIONS.forEach(function (clothing) {
    PLAYER_CLOTHING_OPTIONS[clothing.id] = clothing;
});

/**
 * @type {TitleClothingSelectionIcon[]}
 */
var titleClothingSelectors = [];

 /* Keep in sync with total number of calls to beginStartupStage */
var totalLoadStages = 6;
var curLoadStage = -1;

/**********************************************************************
 *****                    Start Up Functions                      *****
 **********************************************************************/

/************************************************************
 * Functions for the startup loading progress menu.
 ************************************************************/

function beginStartupStage (label) {
    curLoadStage++;
    $gameLoadLabel.text(label);
    updateStartupStageProgress(0, 1);
}

function updateStartupStageProgress (curItems, totalItems) {
    /*
     * Add the overall loading progress for all prior stages (curLoadStage / totalLoadStages)
     * to a fraction of (1 / totalLoadStages).
     * (1 / totalLoadStages) * (curItems / totalItems) == curItems / (totalItems * totalLoadStages)
     */
    var progress = Math.floor(100 * (
        (curLoadStage / totalLoadStages) +
        (curItems / (totalItems * totalLoadStages))
    ));
    $gameLoadProgress.text(progress.toString(10));
}

function finishStartupLoading () {
    $("#warning-start-container").removeAttr("hidden");
    $("#warning-load-container").hide();
}

/**
 * @param {PlayerClothing} clothing 
 */
function TitleClothingSelectionIcon (clothing) {
    this.clothing = clothing;
    $(this.elem = clothing.createSelectionElement())
        .addClass("title-content-button").click(this.onClick.bind(this))
        .on('touchstart', function() {
            $(this.elem).tooltip('show');
        }.bind(this)).tooltip({
            delay: 50,
            title: function() { return clothing.tooltip(); }
        });
}

TitleClothingSelectionIcon.prototype.visible = function () {
    if (this.clothing.isAvailable()) {
        return true;
    }

    if (this.clothing.applicable_genders !== "all" && humanPlayer.gender !== this.clothing.applicable_genders) {
        return false;
    }

    if (this.clothing.collectible) {
        return !this.clothing.collectible.hidden;
    }

    return false;
}

TitleClothingSelectionIcon.prototype.update = function () {
    $(this.elem).removeClass("locked selected");
    if (!this.clothing.isAvailable()) {
        $(this.elem).addClass("locked");
    }
    if (this.clothing.isSelected()) {
        $(this.elem).addClass("selected");
    }
}

TitleClothingSelectionIcon.prototype.onClick = function () {
    if (this.clothing.isAvailable()) {
        this.clothing.setSelected(!this.clothing.isSelected());
        this.update();
        updateClothingCount();
    }
}

function setupTitleClothing () {
    var prevScroll = 0;
    $('#title-clothing-container').on('scroll', function() {
        if (Math.abs(this.scrollTop - prevScroll) > this.clientHeight / 4) {
            $("#title-clothing-container .player-clothing-select").tooltip('hide');
            prevScroll = this.scrollTop;
        }
    }).on('show.bs.tooltip', function(ev) {
        $("#title-clothing-container .player-clothing-select").not(ev.target).tooltip('hide');
        prevScroll = this.scrollTop;
    });

    loadedOpponents.forEach(function (opp) {
        if (!opp.has_collectibles || !opp.collectibles) return;

        opp.collectibles.forEach(function (collectible) {
            var clothing = collectible.clothing;
            if (
                (!clothing || !PLAYER_CLOTHING_OPTIONS[clothing.id]) ||
                (collectible.status && !includedOpponentStatuses[collectible.status])
            ) {
                return;
            }

            var selector = new TitleClothingSelectionIcon(clothing);
            titleClothingSelectors.push(selector);
        });
    });

    DEFAULT_CLOTHING_OPTIONS.forEach(function (clothing) {
        var selector = new TitleClothingSelectionIcon(clothing);
        titleClothingSelectors.push(selector);
    });

    updateClothingCount();
}

/**********************************************************************
 *****                   Interaction Functions                    *****
 **********************************************************************/

/************************************************************
 * The player clicked on one of the gender icons on the title
 * screen, or this was called by an internal source.
 ************************************************************/
function changePlayerGender (gender) {
    save.savePlayer();
    humanPlayer.gender = gender;
    save.loadPlayer();
    updateTitleScreen();
    updateSelectionVisuals(); // To update epilogue availability status
}

$('.title-gender-button').on('click', function(ev) {
    changePlayerGender($(ev.target).data('gender'));
});

function createClothingSeparator () {
    var separator = document.createElement("hr");
    separator.className = "clothing-separator";
    return separator;
}

/************************************************************
 * Updates the gender dependent controls on the title screen.
 ************************************************************/
function updateTitleScreen () {
    $titleContainer.removeClass('male female').addClass(humanPlayer.gender);
    $playerTagsModal.removeClass('male female').addClass(humanPlayer.gender);
    $('.title-gender-button').each(function() {
        $(this).toggleClass('selected', $(this).data('gender') == humanPlayer.gender);
    });

    var availableSelectors = [];
    var defaultSelectors = [];
    var lockedSelectors = [];

    titleClothingSelectors.forEach(function (selector) {
        var clothing = selector.clothing;
        $(selector.elem).detach();

        if (!selector.visible()) {
            return;
        }

        if (selector.clothing.collectible) {
            if (selector.clothing.isAvailable()) {
                availableSelectors.push(selector);
            } else {
                lockedSelectors.push(selector);
            }
        } else {
            defaultSelectors.push(selector);
        }

        selector.update();
    });

    $("#title-clothing-container").empty();

    if (availableSelectors.length > 0) {
        $("#title-clothing-container").append(availableSelectors.map(function (s) {
            return s.elem;
        })).append(createClothingSeparator());
    }

    $("#title-clothing-container").append(defaultSelectors.map(function (s) {
        return s.elem;
    }));

    if (lockedSelectors.length > 0) {
        $("#title-clothing-container").append(createClothingSeparator()).append(
            lockedSelectors.map(function (s) {
                return s.elem;
            })
        );
    }
    updateClothingCount();
}

/************************************************************
 * The player clicked on one of the size icons on the title
 * screen, or this was called by an internal source.
 ************************************************************/
function changePlayerSize (size) {
    humanPlayer.size = size;
    $sizeBlocks[humanPlayer.gender].find('.title-size-button').each(function() {
        $(this).toggleClass('selected', $(this).data('size') == size);
    });
}

$('.title-size-block').on('click', '.title-size-button', function(ev) {
    changePlayerSize($(ev.target).data('size'));
});

/**************************************************************
 * Add tags to the human player based on the selections in the tag
 * dialog and the size.
 **************************************************************/
function setPlayerTags () {
    var playerTagList = ['human', 'human_' + humanPlayer.gender,
                         humanPlayer.size + (humanPlayer.gender == 'male' ? '_penis' : '_breasts')];

    for (category in playerTagSelections) {
        var sel = playerTagSelections[category];
        if (!(category in playerTagOptions)) continue;
        playerTagOptions[category].values.some(function (choice) {
            if (playerTagOptions[category].type == 'range') {
                if (sel > choice.to) return false;
            } else {
                if (sel != choice.value) return false;
            }
            playerTagList.push(choice.value);
            return true;
        });
    }
    /* applies tags to the player*/
    console.log(playerTagList);
    humanPlayer.baseTags = playerTagList.map(canonicalizeTag);
    humanPlayer.updateTags();
}

/************************************************************
 * The player clicked on the advance button on the title
 * screen.
 ************************************************************/
function validateTitleScreen () {
    /* determine the player's name */
    var playerName = '';

    if ($nameField.val() != "") {
        playerName = $nameField.val();
    } else if (humanPlayer.gender == "male") {
        playerName = "Mister";
    } else if (humanPlayer.gender == "female") {
        playerName = 'Miss';
    }

    humanPlayer.first = playerName;
    humanPlayer.label = playerName;

    $gameLabels[HUMAN_PLAYER].text(humanPlayer.label);

    /* count clothing */
    var clothingItems = save.selectedClothing();
    console.log(clothingItems.length);

    /* ensure the player is wearing enough clothing */
    if (clothingItems.length > 8) {
        $warningLabel.html("You cannot wear more than 8 articles of clothing. Cheater.");
        return;
    }

    /* dress the player */
    wearClothing();
    setPlayerTags();

    save.savePlayer();
    console.log(players[0]);

    setLocalDayOrNight();
    updateAllBehaviours(null, null, SELECTED);
    updateSelectionVisuals();

    Sentry.setTag("screen", "select-main");
    screenTransition($titleScreen, $selectScreen);

    updateAnnouncementDropdown();
    showAnnouncements();

    if (curResortEvent && !curResortEvent.resort.checkCharacterThreshold()) {
        curResortEvent.resort.setFlag(false);
    }
}

/**********************************************************************
 *****                    Additional Functions                    *****
 **********************************************************************/

/************************************************************
 * Takes all of the clothing selected by the player and adds it,
 * in a particular order, to the list of clothing they are wearing.
 ************************************************************/
function wearClothing () {
    var position = [[], [], []];
    var typeIdx = {
        "important": 0,
        "major": 1,
        "minor": 2,
        "extra": 3,
    };

    save.selectedClothing().sort(function (a, b) {
        return typeIdx[a.type] - typeIdx[b.type];
    }).forEach(function (clothing) {
        if (clothing.position == UPPER_ARTICLE) {
            position[0].push(clothing);
        } else if (clothing.position == LOWER_ARTICLE) {
            position[1].push(clothing);
        } else {
            position[2].push(clothing);
        }
    });

    /* clear player clothing array */
    humanPlayer.clothing = [];

    /* wear the clothing is sorted order */
    for (var i = 0; i < position[0].length || i < position[1].length; i++) {
        /* wear a lower article, if any remain */
        if (i < position[1].length) {
            humanPlayer.clothing.push(position[1][i]);
        }

        /* wear an upper article, if any remain */
        if (i < position[0].length) {
            humanPlayer.clothing.push(position[0][i]);
        }
    }

    /* wear any other clothing */
    for (var i = 0; i < position[2].length; i++) {
        humanPlayer.clothing.push(position[2][i]);
    }

    humanPlayer.initClothingStatus();

    /* update the visuals */
    displayHumanPlayerClothing();
}

/************************************************************
 * Update the warning text to say how many items of clothing are being worn.
 ************************************************************/
function updateClothingCount(){
    /* the amount of clothing being worn */
    var clothingCount = save.selectedClothing();

    $warningLabel.html(`Select from 0 to 8 articles. Wear whatever you want. (${clothingCount.length}/8)`);
    return;
}
