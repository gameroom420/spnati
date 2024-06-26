/********************************************************************************
 This file contains the variables and functions that forms the core of the game.
 Anything that is needed game-wide is kept here.
 ********************************************************************************/

/**********************************************************************
 * Game Wide Constants
 **********************************************************************/

/* General Constants */
var DEBUG = false;
var EPILOGUES_ENABLED = true;
var EPILOGUES_UNLOCKED = false;
var COLLECTIBLES_ENABLED = true;
var COLLECTIBLES_UNLOCKED = false;
var CARD_DECKS_ENABLED = false;
var ALT_COSTUMES_ENABLED = true;
var DEFAULT_COSTUME_SETS = new Set();
var BASE_FONT_SIZE = 14;
var BASE_SCREEN_WIDTH = 100;
var UI_FONT_WEIGHT = 500;
var UI_FONT_WIDTH = 100;
var UI_THEME = "default";

var CURRENT_VERSION = undefined;
var VERSION_COMMIT = undefined;
var VERSION_TAG = undefined;
var COMMITS_SINCE_TAG = undefined;
var BUILD_TIMESTAMP = undefined;

var DEFAULT_FILL = undefined;
var FILL_DISABLED = false;
var TESTING_MAX_AGE = 14 * 86400 * 1000; // 14 days
var TESTING_MIN_NUMBER = 10;
var TESTING_NTH_MOST_RECENT_UPDATE;

/* Game Wide Constants */
var HUMAN_PLAYER = 0;

/* Directory Constants */
var IMG = 'img/';

/*var OPP = 'opponents/';
#The "OPP" folder abbreviation was used to slightly shorten a few lines in spniSelect that looked for opponents in the opponents folder.
#Now that opponents can be specified in any folder, this is no longer required.*/

/* Gender Images */
var MALE_SYMBOL = IMG + 'male.svg';
var FEMALE_SYMBOL = IMG + 'female.svg';
var FUTANARI_SYMBOL = IMG + 'futanari.svg';

var includedOpponentStatuses = {};
var alternateCostumeSets = {};

var versionInfo = null;

/* game table */
var tableOpacity = 1;
var hiddenTableOpacity = 0.3;
$gameTable = $('#game-table');
$gameTableHidden = $('#game-hidden-area');

/* useful variables */
var BLANK_PLAYER_IMAGE = "opponents/blank.png";

/* player array */
var players = Array(5);
Object.defineProperty(players, 'opponents', {
    get: function() {
        return this.slice(1);
    }
});
var humanPlayer;

/* Current timeout ID, so we can cancel it when restarting the game in order to avoid trouble. */
var timeoutID;


/**********************************************************************
 * Game Wide Global Variables
 **********************************************************************/

var table = new Table();
var jsErrors = [];
var sessionID = '';
var gameID = '';
var generalCollectibles = [];
var isLocal = false;
var isMainSite = true;

/**********************************************************************
 * Screens & Modals
 **********************************************************************/

/* Screens */
$titleScreen = $('#title-screen');
$selectScreen = $('#main-select-screen');
$individualSelectScreen = $('#individual-select-screen');
$groupSelectScreen = $('#group-select-screen');
$gameScreen = $('#game-screen');
$epilogueScreen = $('#epilogue-screen');
$galleryScreen = $('#gallery-screen');

var allScreens = [$titleScreen, $selectScreen, $individualSelectScreen, $groupSelectScreen, $gameScreen, $epilogueScreen, $galleryScreen];

/* Modals */
$helpModal = $('#help-modal');
$creditModal = $('#credit-modal');
$versionModal = $('#version-modal');
$playerTagsModal = $('#player-tags-modal');
$collectibleInfoModal = $('#collectibles-info-modal');
$ioModal = $('#io-modal');
$extrasModal = $('#extras-modal');
$resortModal = $('#resort-modal');
$eventAnnouncementModal = $('#event-announcement-modal');
$characterDebugModal = $("#character-debug-modal");

/* Screen State */
$previousScreen = null;

/* CSS rules for arrow offsets */
var bubbleArrowOffsetRules;

/**********************************************************************
 *****              Overarching Game Flow Functions               *****
 **********************************************************************/

function fuzzyTimeAgo(ts) {
    now = Date.now();

    elapsed_time = now - ts;

    /* Format last update time */
    string = '';
    if (elapsed_time < 5 * 60 * 1000) {
        // <5 minutes ago - display 'just now'
        string = 'just now';
    } else if (elapsed_time < 60 * 60 * 1000) {
        // < 1 hour ago - display minutes since last update
        string = Math.floor(elapsed_time / (60 * 1000))+' minutes ago';
    } else if (elapsed_time < 24 * 60 * 60 * 1000) {
        // < 1 day ago - display hours since last update
        var n_hours = Math.floor(elapsed_time / (60 * 60 * 1000));
        string = n_hours + (n_hours === 1 ? ' hour ago' : ' hours ago');
    } else {
        // otherwise just display days since last update
        var n_days = Math.floor(elapsed_time / (24 * 60 * 60 * 1000));
        string =  n_days + (n_days === 1 ? ' day ago' : ' days ago');
    }
    return string;
}

/************************************************************
 * Loads the initial content of the game.
 ************************************************************/
function initialSetup () {
    /* start by creating the human player object */
    players[HUMAN_PLAYER] = humanPlayer = new Player('human'); //createNewPlayer("human", "", "", "", eGender.MALE, eSize.MEDIUM, eIntelligence.AVERAGE, 20, undefined, [], null);
    humanPlayer.slot = HUMAN_PLAYER;
    humanPlayer.resetState();

    /* Generate a random session ID. */
    sessionID = generateRandomID();

    /* enable table opacity */
    tableOpacity = 1;
    $gameTable.css({opacity:1});

    /* Attempt to detect broken images as caused by running SPNATI from an invalid archive. */
    detectBrokenOffline();

    /* load global origin variables */
    var origin = getReportedOrigin();
    isLocal = origin.includes("localhost") || origin.includes("local filesystem");
    isMainSite = origin.includes("spnati.net");
    
    /* Make sure that the config file is loaded before processing the
     *  opponent list, so that includedOpponentStatuses is populated.
     *
     * Also ensure that the config file is loaded before initializing Sentry,
     * which requires the commit SHA.
     */
    loadConfigFile().then(function () {
        try {
            sentryInit();
        } catch (err) {
            captureError(err);
        }
        /* Make sure that save data is loaded before updateTitleScreen(),
         * since the latter uses selectedClothing.
         */
        save.loadLocalStorage();
    }).then(loadEventData).then(function () {
        return Promise.all([
            loadBackgrounds(), loadCustomDecks()
        ]);
    }).then(function () {
        save.load();
        return loadVersionInfo();
    }).then(metadataIndex.loadIndex.bind(metadataIndex)).then(loadSelectScreen).then(loadAllCollectibles).then(function () {
        selectTitleCandy();
        setupTitleClothing();
        finishStartupLoading();

        if (!EPILOGUES_ENABLED && !COLLECTIBLES_ENABLED) {
            $('.title-gallery-edge').css('visibility', 'hidden');
        }
        updateTitleScreen();
        updateAnnouncementDropdown();
    });

    Sentry.setTag("screen", "warning");

    /* show the title screen */
    $titleScreen.show();
    $('.warning-container').focus();
    autoResizeFont();
    /* set up future resizing */
    window.onresize = autoResizeFont;

    /* Construct a CSS rule for every combination of arrow direction, screen, and pseudo-element */
    bubbleArrowOffsetRules = [];
    var targetCssSheet = document.getElementById("spniStyleSheet").sheet;

    for (var i = 1; i <= 4; i++) {
        var pair = [];
        [["up", "down"], ["left", "right"]].forEach(function(p) {
            var index = targetCssSheet.cssRules.length;
            var rule = p.map(function(d) {
                return ["select", "game"].map(function(s) {
                    return '#'+s+'-bubble-'+i+'.arrow-'+d+'::before';
                }).join(', ');
            }).join(', ') + ' {}';
            targetCssSheet.insertRule(rule, index);
            pair.push(targetCssSheet.cssRules[index]);
        });
        bubbleArrowOffsetRules.push(pair);
    }
    $(document).keydown(function(ev) {
        if (ev.key == "Tab") {  // Tab
            $("body").addClass('focus-indicators-enabled');
        }
    });
    $(document).keyup(function(ev) {
        if (ev?.key?.toLowerCase() == 'f' && !ev.shiftKey
            && !$(document.activeElement).is('input, select, textarea')) {
            toggleFullscreen();
        } else if (ev.key == "F1") {
            showHelpModal();
            ev.preventDefault();
        } else if (ev.key == "Escape") {
            $("body").removeClass('focus-indicators-enabled');
        }
    });
    $(document).mousedown(function(ev) {
        $("body").removeClass('focus-indicators-enabled');
    });
    $(window).on('beforeunload', function() {
        if (inGame) {
            event.preventDefault();
            event.returnValue = '';
        }
    });

    window.addEventListener("unload", function () {
        if ((document.visibilityState === "hidden") && inGame && !gameOver) {
            recordInterruptedGameEvent(true);
        }
    });

    $('[data-toggle="tooltip"]').tooltip({ delay: { show: 200 } });
}

function loadVersionInfo () {
    $('.substitute-version').text('Unknown Version');

    beginStartupStage("Version Info");
    
    return fetchXML("version-info.xml").then(function($xml) {
        versionInfo = $xml;
        var versionElem = versionInfo.children('current');
        CURRENT_VERSION = versionElem.attr('version');

        Sentry.setTag("game_version", CURRENT_VERSION);
        
        var displayedVersion = 'v' + CURRENT_VERSION;

        /* As a sanity check, make sure that the tag git-describe found corresponds to the version number we think we're running. */
        if (VERSION_TAG && VERSION_TAG.startsWith("v" + CURRENT_VERSION)) {
            /* The output of git-describe is in the form:
             * [tag name]-[commits since tag]-g[ID of commit]
             * (for example, "v12.137.0-9-gfbf2fe588e").
             */
            let commit_sep = VERSION_TAG.lastIndexOf("-");
            let commit_ctr_sep = VERSION_TAG.lastIndexOf("-", commit_sep-1);
            COMMITS_SINCE_TAG = parseInt(VERSION_TAG.substring(commit_ctr_sep+1, commit_sep), 10);

            /* Display commit count if both valid *and* nonzero. */
            if (COMMITS_SINCE_TAG) {
                displayedVersion = displayedVersion + "-" + COMMITS_SINCE_TAG;
            }
        }

        $('.substitute-version').text(displayedVersion);
        console.log("Running SPNATI version "+CURRENT_VERSION);

        var version_ts = parseInt(versionElem.attr("build-timestamp"), 10);
        if (version_ts) {
            BUILD_TIMESTAMP = version_ts;
        } else {
            version_ts = versionInfo.find('>changelog > version[number=\"'+CURRENT_VERSION+'\"]').attr('timestamp');
            version_ts = parseInt(version_ts, 10);
        }
        
        $('.substitute-version-time').text('(updated '+fuzzyTimeAgo(version_ts)+')');

        $('.version-button').click(showVersionModal);
    }).catch(function (err) {
        console.error("Failed to load version info");
        captureError(err);
    });
}


function loadConfigFile () {
    beginStartupStage("Configuration");

    return fetchXML("config.xml").then(function($xml) {
        var _epilogues = $xml.children('epilogues').text();
        if(_epilogues.toLowerCase() === 'false') {
            EPILOGUES_ENABLED = false;
            console.log("Epilogues are disabled.");
            $("#title-gallery-edge").hide();
        } else {
            console.log("Epilogues are enabled.");
            EPILOGUES_ENABLED = true;
        }

        var _epilogues_unlocked = $xml.children('epilogues-unlocked').text().trim();
        if (_epilogues_unlocked.toLowerCase() === 'true') {
            EPILOGUES_UNLOCKED = true;
            console.error('All epilogues unlocked in config file. You better be using this for development only and not cheating!');
        } else {
            EPILOGUES_UNLOCKED = false;
        }

        var _debug = $xml.children('debug').text();
        if (_debug === "true") {
            DEBUG = true;
            console.log("Debugging is enabled");
        }
        else {
            DEBUG = false;
            console.log("Debugging is disabled");
        }

        var _default_fill_mode = $xml.children('default-fill').text();
        if (!_default_fill_mode || _default_fill_mode === 'none') {
            DEFAULT_FILL = undefined;
            console.log("Startup table filling disabled");
        } else {
            DEFAULT_FILL = _default_fill_mode;
            console.log("Using startup table fill mode " + DEFAULT_FILL + '.');
        }

        var _game_commit = $xml.children('commit').text();
        if (_game_commit) {
            VERSION_COMMIT = _game_commit;
            console.log("Running SPNATI commit "+VERSION_COMMIT+'.');
        } else {
            console.log("Could not find currently deployed Git commit!");
        }

        var _version_tag = $xml.children('version-tag').text();
        if (_version_tag) {
            VERSION_TAG = _version_tag;
            console.log("Running SPNATI production version " + VERSION_TAG + '.');
        } else {
            console.log("Could not find currently deployed production version tag!");
        }

        var _default_bg = $xml.children('default-background').text();
        if (_default_bg) {
            defaultBackgroundID = _default_bg;
            console.log("Using default background: "+defaultBackgroundID);
        } else {
            defaultBackgroundID = 'inventory';
            console.log("No default background ID set, defaulting to 'inventory'...");
        }

        var _alts = $xml.children('alternate-costumes').text();

        if(_alts === "false") {
            ALT_COSTUMES_ENABLED = false;
            console.log("Alternate costumes disabled");
        } else {
            console.log("Alternate costumes enabled");

            DEFAULT_COSTUME_SETS = new Set($xml.find('default-costume-set').map(function (index, $elem) {
                return $elem.text();
            }).get());

            DEFAULT_COSTUME_SETS.forEach(function (setId) {
                console.log("Added default alternate costume set: " + setId);
                alternateCostumeSets[setId] = true;
            });

            $xml.children('alternate-costume-sets').each(function () {
                var set = $(this).text();
                alternateCostumeSets[set] = true;
                if (set === 'all') {
                    console.log("Including all alternate costume sets");
                } else {
                    console.log("Including alternate costume set: "+set);
                }
            });
        }

        MANUAL_EVENTS = new Set();
        OVERRIDE_EVENTS = new Set();

        $xml.find("event").each(function (index, elem) {
            var $elem = $(elem);
            var eventID = $elem.text();
            if (($elem.attr("override") || "").trim().toLowerCase() === "true") {
                console.log("Manually activating event: " + eventID + " (as override)");
                OVERRIDE_EVENTS.add(eventID);
            } else {
                console.log("Manually activating event: " + eventID);
                MANUAL_EVENTS.add(eventID);
            }
        });
        
        COLLECTIBLES_ENABLED = false;
        COLLECTIBLES_UNLOCKED = false;
        
        if ($xml.children('collectibles').text() === 'true') {
            COLLECTIBLES_ENABLED = true;
            console.log("Collectibles enabled");
            
            if ($xml.children('collectibles-unlocked').text() === 'true') {
                COLLECTIBLES_UNLOCKED = true;
                console.log("All collectibles force-unlocked");
            }
        } else {
            console.log("Collectibles disabled");
        }
        CARD_DECKS_ENABLED = false;
        var _card_decks_enabled = $xml.children('custom-cards').text();
        if (_card_decks_enabled.toLowerCase() === 'true') {
            console.log("Card deck customization active.");
            CARD_DECKS_ENABLED = true;
        } else {
            CARD_DECKS_ENABLED = false;
            console.log("Card deck customization disabled.");
        }
        
        DEFAULT_CARD_DECK = $xml.children('default-card-deck').text() || 'default';
        console.log("Using default card deck: " + DEFAULT_CARD_DECK);

        includedOpponentStatuses.online = true;
        $xml.children('include-status').each(function() {
            includedOpponentStatuses[$(this).text()] = true;
            console.log("Including", $(this).text(), "opponents");
        });

        var _testing_max_age_days = Number.parseFloat($xml.children('testing-max-age').text());
        if (!Number.isNaN(_testing_max_age_days)) {
            TESTING_MAX_AGE = _testing_max_age_days * 86400 * 1000;
        }
        var _testing_min_number = Number.parseInt($xml.children('testing-min-number').text());
        if (!Number.isNaN(_testing_min_number)) {
            TESTING_MIN_NUMBER = _testing_min_number;
        }
    }).catch(function (err) {
        console.error("Failed to load configuration");
        captureError(err);
    });
}

/**
 * Attempt to detect common ways of incorrectly running the offline version.
 * Specifically, we check the following:
 * - Can we load resources using XHR?
 * - Have image LFS pointers been properly replaced with actual content?
 * If either of these checks fail, the broken offline modal is shown.
 * 
 * @returns {Promise<void>}
 */
function detectBrokenOffline() {
    $("#broken-offline-modal .section").hide();

    var p1 = fetch('img/enter.png', { method: "GET" }).then(function (resp) {
        if (resp.status < 200 || resp.status > 299) {
            throw new Error();
        } else {
            return resp.arrayBuffer();
        }
    }).catch(function () {
        $("#broken-xhr-section").show();
        $("#broken-offline-modal").modal('show');
    }).then(function (buf) {
        if (!buf) return;
        var bytes = new Uint8Array(buf, 0, 8);
        var header = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
        if (!header.every(function (v, i) { return bytes[i] === v; })) {
            throw new Error();
        }
    }).catch(function () {
        $("#broken-images-section").show();
        $("#broken-offline-modal").modal('show');
    });

    var p2 = new Promise(function (resolve, reject) {
        var img = new Image();
        img.onerror = reject.bind(null);
        img.onload = resolve.bind(null);
        img.src = "img/enter.png";
    }).catch(function () {
        $("#broken-images-section").show();
        $("#broken-offline-modal").modal('show');
    });

    return Promise.all([p1, p2]);
}

function enterTitleScreen() {
    $warningContainer.hide();
    $titleContainer.show();
    $('.title-candy').show();
    Sentry.setTag("screen", "title");
}

/************************************************************
 * Transitions between two screens.
 ************************************************************/
function screenTransition (first, second) {
    if (first.data('keyhandler')) {
        $(document).off('keyup', first.data('keyhandler'));
    }
    first.hide();
    second.show();
    if (second.data('keyhandler')) {
        $(document).on('keyup', second.data('keyhandler'));
    }
    autoResizeFont();
}

/************************************************************
 * Switches to the next screen based on the screen provided.
 ************************************************************/
function advanceToNextScreen (screen) {
    if (screen == $titleScreen) {
        /* advance to the select screen */
        screenTransition(screen, $selectScreen);
    } else if (screen == $selectScreen) {
        /* advance to the main game screen */
        loadGameScreen();
        screenTransition(screen, $gameScreen);
        $mainButton.focus();
    }
}

/************************************************************
 * Switches to the last screen based on the screen provided.
 ************************************************************/
function returnToPreviousScreen (screen) {
    if (screen == $selectScreen) {
        /* return to the title screen */
        screenTransition($selectScreen, $titleScreen);
    }
}

/************************************************************
 * Resets the game state so that the game can be restarted.
 ************************************************************/
function resetPlayers () {
    players.forEach(function(p) {
        p.resetState();
    });
}

/************************************************************
 * Restarts the game.
 ************************************************************/
function restartGame () {
    Sentry.addBreadcrumb({
        category: 'ui',
        message: 'Returning to title screen.',
        level: 'info'
    });

    Sentry.setTag("screen", "title");
    Sentry.setTag("epilogue_player", undefined);
    Sentry.setTag("epilogue", undefined);
    Sentry.setTag("epilogue_gallery", undefined);

    if (!gameOver) {
        recordInterruptedGameEvent(false);
    }

    clearTimeout(timeoutID); // No error if undefined or no longer valid
    timeoutID = autoForfeitTimeoutID = undefined;
    stopCardAnimations();
    $('link[href^="opponents/"]').remove();
    resetPlayers();
    currentRound = -1;

    /* enable table opacity */
    tableOpacity = 1;
    $gameTable.css({opacity:1});
    $gamePlayerCardArea.show();
    $gamePlayerClothingArea.css('display', '');  /* Reset to default so as not to interfere with 
                                                    switching between classic and minimal UI. */
    inGame = false;
    autoAdvancePaused = false;

    Sentry.setTag("in_game", false);

    /* trigger screen refreshes */
    updateSelectionVisuals();
    updateAllGameVisuals();
    selectTitleCandy();
    updateTitleScreen();

    forceTableVisibility(true);

    /* there is only one call to this right now */
    $epilogueSelectionModal.hide();
    clearEpilogue();
    screenTransition($epilogueScreen, $titleScreen);
    screenTransition($gameScreen, $titleScreen);
    autoResizeFont();
}

/**********************************************************************
 *****                    Interaction Functions                   *****
 **********************************************************************/


var SEMVER_RE = /[vV]?(\d+)\.(\d+)(?:\.(\d+))?(?:\-([a-zA-Z0-9\-]+(?:\.[a-zA-Z0-9\-])*))?(?:\+([a-zA-Z0-9\-]+(?:\.[a-zA-Z0-9\-]+)*))?/;
function parseSemVer (versionString) {
    var m = versionString.match(SEMVER_RE);
    if (!m) return null;
    
    var ver = {
        'major': parseInt(m[1], 10) || 0,
        'minor': parseInt(m[2], 10) || 0,
        'patch': parseInt(m[3], 10) || 0,
    }
    
    if (m[4]) {
        ver.prerelease = m[4].split('.');
    }
    
    return ver;
}

/* Implements semver precedence rules, as specified in the Semantic Versioning 2.0.0 spec. */
function compareVersions (v1, v2) {
    var m1 = parseSemVer(v1);
    var m2 = parseSemVer(v2);
    
    // compare major - minor - patch first, in that order
    if (m1.major !== m2.major) return (m1.major > m2.major) ? 1 : -1;
    if (m1.minor !== m2.minor) return (m1.minor > m2.minor) ? 1 : -1;
    if (m1.patch !== m2.patch) return (m1.patch > m2.patch) ? 1 : -1;
    
    // prerelease versions always have less precedence than release versions
    if (!m1.prerelease && m2.prerelease) return 1;
    if (!m2.prerelease && m1.prerelease) return -1;
    
    // Compare pre-release identifiers from left to right
    for (var i=0;i<Math.min(m1.prerelease.length, m2.prerelease.length);i++) {
        var pr1 = parseInt(m1.prerelease[i], 10);
        var pr2 = parseInt(m2.prerelease[i], 10);
        
        if (m1.prerelease[i] !== m2.prerelease[i]) {
            // if both are numerical or both are non-numeric
            if (isNaN(pr1) === isNaN(pr2)) return (pr1 > pr2) ? 1 : -1;
            
            // otherwise, if we're comparing numeric to non-numeric,
            // numeric PR identifiers always have less precedence than non-numeric
            return isNaN(pr1) ? 1 : -1;
        }
    }
    
    // All pre-release identifiers to now compared equal
    // in this case, the version with a larger set of pre-release fields has
    // higher precedence
    if (m1.prerelease.length !== m2.prerelease.length) {
        return (m1.prerelease.length > m2.prerelease.length) ? 1 : -1;
    }
    
    // If all else fails, both versions compare equal
    return 0;
}

$creditModal.on('shown.bs.modal', function() {
    $('#credit-modal-button').focus();
});

function createVersionRow (timestamp, version, text) {
    var row = document.createElement('tr');
    var versionCell = document.createElement('td');
    var dateCell = document.createElement('td');
    var logTextCell = document.createElement('td');

    versionCell.className = 'changelog-version-label';
    dateCell.className = 'changelog-date-label';
    logTextCell.className = 'changelog-entry-text';

    if (timestamp) {
        var date = new Date(timestamp);
        var locale = window.navigator.userLanguage || window.navigator.language
        dateCell.innerText = date.toLocaleString(locale, {'dateStyle': 'medium', 'timeStyle': 'short'});
    }

    versionCell.innerText = version;
    logTextCell.innerText = text;

    row.appendChild(versionCell);
    row.appendChild(dateCell);
    row.appendChild(logTextCell);

    return row;
}

/************************************************************
 * The player clicked the version button. Shows the version modal.
 ************************************************************/
function showVersionModal () {
    var $changelog = $('#changelog-container');
    var entries = [];
    
    /* Get changelog info: */
    versionInfo.find('> changelog > version').each(function (idx, elem) {
        entries.push({
            version: $(elem).attr('number'),
            timestamp: parseInt($(elem).attr('timestamp'), 10) || undefined,
            text: $(elem).text()
        });
    });
    
    /* Construct the version modal DOM: */
    $changelog.empty().append(entries.sort(function (e1, e2) {
        // Sort in reverse-precedence order
        return compareVersions(e1.version, e2.version) * -1;
    }).map(function (ent) {
        return createVersionRow(ent.timestamp, ent.version, ent.text);
    }));

    if (COMMITS_SINCE_TAG) {
        $changelog.prepend(createVersionRow(
            BUILD_TIMESTAMP,
            CURRENT_VERSION + "-" + COMMITS_SINCE_TAG,
            COMMITS_SINCE_TAG + " commits have been pushed since the last version number update. These represent smaller and more frequent updates for which we haven't created a version log entry (yet)."
        ));
    }
    
    $versionModal.modal('show');
}

$versionModal.on('shown.bs.modal', function() {
    $('#version-modal-button').focus();
});

/************************************************************
 * The player clicked the help / FAQ button. Shows the help modal.
 ************************************************************/
function showHelpModal () {
    $helpModal.modal('show');
}

function gotoHelpPage (toPage) {
    var curPage = $helpModal.attr('data-current-page');
    curPage = parseInt(curPage, 10) || 1;
    
    if (toPage === 'prev') {
        curPage = (curPage > 1) ? curPage-1 : 1;
    } else if (toPage === 'next') {
        curPage = (curPage < 8) ? curPage+1 : 8;
    } else {
        curPage = toPage;
    }
    
    $helpModal.attr('data-current-page', curPage);
    $('.help-page').hide();
    $('.help-page[data-page="'+curPage+'"]').show();
    $('.help-page-select').removeClass('active');
    $('.help-page-select[data-page="'+curPage+'"]').addClass('active');
}

$('.help-page-select').click(function (ev) {
    gotoHelpPage($(ev.target).attr('data-select-page'));
})

/************************************************************
 * The player clicked the Discord button. Shows the server links modal.
 ************************************************************/
function showDiscordModal() {
    $("#discord-modal").modal("show");
}

/************************************************************
 * The player clicked the player tags button. Shows the player tags modal.
 ************************************************************/
function showPlayerTagsModal () {
    if (document.forms['player-tags'].elements.length <= 6) {
        // Safari doesn't support color inputs properly!
        var hairColorPicker = document.getElementById('hair_color_picker');
        var selectionType;
        try {
            selectionType = typeof hairColorPicker.selectionStart;
        } catch(e) {
            selectionType = null;
        }
        for (var choiceName in playerTagOptions) {
            var replace = (choiceName != 'skin_color' || selectionType === 'number');
            var $existing = $('form#player-tags [name="'+choiceName+'"]');
            if (!replace && $existing.length) continue;
            var $select = $('<select>', { name: choiceName, id: 'player-tag-choice-'+choiceName });
            $select.append('<option>', playerTagOptions[choiceName].values.map(function(opt) {
                return $('<option>').val(opt.value).addClass(opt.gender).append(opt.text || opt.value.replace(/_/g, ' ').initCap());
            }));
            if ($existing.length) {
                $existing.parent().replaceWith($select);
            } else {
                var $label = $('<div class="player-tag-select">');
                $label.append($('<label>', { 'for': 'player-tag-choice-' + choiceName,
                                             'text': 'Choose your ' + choiceName.replace(/_/g, ' ') + ':'}));
                if (playerTagOptions[choiceName].gender) {
                    $select.addClass(playerTagOptions[choiceName].gender);
                    $label.addClass(playerTagOptions[choiceName].gender);
                }
                $label.append($select);
                $('form#player-tags').append($label);
            }
        }

        var rgb2hsv = function(rgb) {
          var r = parseInt(rgb.slice(1,3), 16)/255;
          var g = parseInt(rgb.slice(3,5), 16)/255;
          var b = parseInt(rgb.slice(5), 16)/255;

          var min = Math.min(r, Math.min(g,b));
          var max = Math.max(r, Math.max(g,b));

          if (max === 0) {
            return [0,0,0];
          }

          var maxOffset = max === r ? 0 : (max === g ? 2 : 4);
          var delta = max === r ? g-b : (max === g ? b-r : r-g);

          var h = 60 * (maxOffset + delta / (max - min));
          if (h < 0) {
            h += 360;
          }

          return [h, (max - min) / max * 100, max * 100];
        }

        /* convert the raw colors to corresponding tags and display next to selector */
        $('input[type=color]').on('input', function() {
            var h, s, v;
            [h,s,v] = rgb2hsv(this.value);

            var tag;
            color2tag:
            if (this.id === 'hair_color_picker') {
              if (v < 10) {
                tag = 'black_hair';
                break color2tag;
              }

              if (s < 25) {
                if (v < 30) {
                  tag = 'black_hair';
                } else {
                  tag = 'white_hair';
                }
                break color2tag;
              }

              if (s < 50 && h > 20 && h < 50) {
                tag = 'brunette';
                break color2tag;
              }

              if (h < 50) {
                tag = 'ginger';
              } else if (h < 65) {
                tag = 'blonde';
              } else if (h < 325) {
                if (h < 145) {
                  tag = 'green_hair';
                } else if (h < 255) {
                  tag = 'blue_hair';
                } else if (h < 290) {
                  tag = 'purple_hair';
                } else {
                  tag = 'pink_hair';
                }
              } else {
                tag = 'ginger';
              }
            } else if (this.id === 'eye_color_picker') {

              if (v < 25) {
                tag = 'dark_eyes';
                break color2tag;
              }

              if (s < 20) {
                tag = 'pale_eyes';
                break color2tag;
              }

              if (h < 15) {
                tag = 'red_eyes';
              } else if (h < 65) {
                tag = 'amber_eyes';
              } else if (h < 145) {
                tag = 'green_eyes';
              } else if (h < 255) {
                tag = 'blue_eyes';
              } else if (h < 325) {
                tag = 'violet_eyes';
              } else {
                tag = 'red_eyes';
              }
            }

            this.previousElementSibling.value = tag || '';
        });

        $('input[name=skin_color]').on('input', function() {
            for (var i = 0; i < playerTagOptions['skin_color'].values.length; i++) {
                if (this.value <= playerTagOptions['skin_color'].values[i].to) {
                    tag = playerTagOptions['skin_color'].values[i].value;
                    break;
                }
            }

            this.previousElementSibling.value = tag || '';
        });

        $('.modal-button.clearSelections').click(function() {
            var formElements = document.forms['player-tags'].elements;
            for (var i = 0; i < formElements.length; i++) {
                if (formElements[i].type !== 'color') {
                    formElements[i].value = '';
                }
            }
        });
    }

    for (var choiceName in playerTagOptions) {
        if (playerTagSelections.hasOwnProperty(choiceName)) {
            $('form#player-tags [name="'+choiceName+'"]').val(playerTagSelections[choiceName]).trigger('input');
        }
    }
    $('#player-tags-confirm').one('click', function() {
        playerTagSelections = {};
        for (var choiceName in playerTagOptions) {
            if (!('gender' in playerTagOptions[choiceName]) || playerTagOptions[choiceName].gender == humanPlayer.gender) {
                var val = $('form#player-tags [name="'+choiceName+'"]').val();
                if (val) {
                    playerTagSelections[choiceName] = val;
                }
            }
        }
    });
    $playerTagsModal.modal('show');
}

/************************************************************
 * The player clicked on a table opacity button.
 ************************************************************/
function toggleTableVisibility () {
    if (tableOpacity > 0) {
        $gameTable.fadeOut(100);
        $gameTableHidden.fadeTo(100, hiddenTableOpacity);
        tableOpacity = 0;
    } else {
        $gameTable.fadeIn(100);
        $gameTableHidden.fadeTo(100, 1.0);
        tableOpacity = 1;
    }
}

function forceTableVisibility(state) {
    if (!state) {
        $gameTable.fadeOut(100);
        tableOpacity = 0;
    } else {
        $gameTable.fadeIn(100);
        $gameTableHidden.fadeTo(100, 1.0);
        tableOpacity = 1;
    }
}

function toggleFullscreen() {
    if (document.fullscreenElement) {
        document.exitFullscreen();
    } else {
        /* handle vendor prefixes for out of date browsers
         * (probably don't need -moz- prefix though, according to Sentry data)
         */
        var d = document.documentElement;
        if (d.requestFullscreen) {
            d.requestFullscreen();
        } else if (d.webkitRequestFullScreen) {
            d.webkitRequestFullScreen();
        } else if (d.msRequestFullscreen) {
            d.msRequestFullscreen();
        }
    }
}

if (!document.fullscreenEnabled && !document.webkitFullscreenEnabled && !document.msRequestFullscreenEnabled) {
    $('.title-fullscreen-button, .game-menu-dropup li:has(#game-fullscreen-button), #epilogue-fullscreen-button').hide();
}
$(':root').on('dblclick', ':input, .dialogue-bubble, .modal-dialog, .selection-card, .bordered, #epilogue-screen', function(ev) {
    ev.stopPropagation();
});
$(':root').on('dblclick', toggleFullscreen);

/************************************************************
 * The player clicked on a Load/Save button.
 ************************************************************/
function showImportModal() {
    $("#export-code").val(save.serializeStorage());
    
    $('#import-invalid-code').hide();
    
    $ioModal.modal('show');

    $('#import-progress').click(function() {
        var code = $("#export-code").val();

        Sentry.addBreadcrumb({
            category: 'ui',
            message: 'Loading save code...',
            level: 'info'
        });

        if (save.deserializeStorage(code)) {
            $ioModal.modal('hide');
        } else {
            $('#import-invalid-code').show();
        }
    });
}

function showExtrasModal() {
    $(".extra-characters-options").prop("hidden", false);
    $('ul.character-status-toggle').each(function() {
        var show = includedOpponentStatuses[$(this).data('status')];
        $(this).children(':has(a[data-value=true])').toggleClass('active', show);
        $(this).children(':has(a[data-value=false])').toggleClass('active', !show);
    });

    /* hide some Extra Opponents options if online version */
    if (isMainSite) {
        $(".event-option").prop("hidden", true);
        $(".duplicate-option").prop("hidden", true);
        $(".broken-option").prop("hidden", true);
    }

    updateTrackingToggles();

    $extrasModal.modal('show');
}

function updateTrackingToggles() {
    let trackingOpts = save.getUsageTrackingInfo();
    $('ul.tracking-status-toggle').each(function() {
        var show = trackingOpts[$(this).data('tracking-option')];
        $(this).children(':has(a[data-value=true])').toggleClass('active', show);
        $(this).children(':has(a[data-value=false])').toggleClass('active', !show);
    });

    $("#tracking-persistent, #tracking-demographics").children().toggleClass("disabled", !trackingOpts.basic);
    if (!trackingOpts.basic) {
        $("#tracking-persistent, #tracking-demographics").children().removeClass("active");
    }
}

$('ul.character-status-toggle').on('click', 'a', function() {
    includedOpponentStatuses[$(this).parents('ul').data('status')] = $(this).data('value');
});

$('ul.tracking-status-toggle').on('click', 'a', function() {
    if ($(this).parents("li").hasClass("disabled")) return;

    var option = $(this).parents('ul').data('tracking-option');
    save.updateUsageTrackingInfo(option, $(this).data('value'));
    updateTrackingToggles();
});

/**********************************************************************
 *****                     Utility Functions                      *****
 **********************************************************************/

/************************************************************
 * Returns a random number in a range.
 ************************************************************/
function getRandomNumber (min, max) {
    return Math.floor(Math.random() * (max - min) + min);
}

function mergeObjects(a, b){
    if(b === undefined || b === null){
        return a;
    }
    else if(a === undefined || a === null){
        return b;
    }
    for(var v in b){
        if (typeof a[v] === 'object') {
            a[v] = mergeObjects(a[v], b[v])
        } else {
            a[v] = b[v];
        }
    }
    return a;
}

function shuffleArray (array) {
    for (var i = array.length - 1; i > 0; i--) {
        var j = getRandomNumber(0, i);
        var tmp = array[i];
        array[i] = array[j];
        array[j] = tmp;
    }
}

/************************************************************
 * Changes the first letter in a string to upper case.
 ************************************************************/
Object.defineProperty(String.prototype, 'initCap', {
    value: function initCap() {
        return this.substr(0, 1).toUpperCase() + this.substr(1);
    }
});

Object.defineProperty(String.prototype, 'escapeHTML', {
    value: function escapeHTML() {
        return document.createElement('span').appendChild(document.createTextNode(this)).parentNode.innerHTML;
    }
});

// Polyfills for IE
if (!String.prototype.startsWith) {
    Object.defineProperty(String.prototype, 'startsWith', {
        value: function(search, pos) {
            pos = !pos || pos < 0 ? 0 : +pos;
            return this.substring(pos, pos + search.length) === search;
        }
    });
}

if (!String.prototype.endsWith) {
    String.prototype.endsWith = function (search, this_len) {
        if (this_len === undefined || this_len > this.length) {
            this_len = this.length;
        }
        return this.substring(this_len - search.length, this_len) === search;
    };
}

if (!Object.entries) {
    Object.entries = function (obj) {
        var ownProps = Object.keys(obj),
            i = ownProps.length,
            resArray = new Array(i); // preallocate the Array
        while (i--)
            resArray[i] = [ownProps[i], obj[ownProps[i]]];

        return resArray;
    };
}

if (!Array.prototype.flat) {
    /**
     * 
     * @param {Array} arr 
     * @param {number} depth 
     * @returns {Array}
     */
    function flatDeep (arr, depth) {
        if (depth > 0) {
            return arr.reduce(function (acc, val) {
                return acc.concat(Array.isArray(val) ? flatDeep(val, depth - 1) : val);
            }, []);
        } else {
            return arr.slice();
        }
    }

    Array.prototype.flat = function (depth) {
        return flatDeep(this, depth || 1);
    }
}


if (!Array.prototype.flatMap) {
    Array.prototype.flatMap = function (callbackFn, thisArg) {
        return this.map(callbackFn, thisArg).flat(1);
    }
}

if (!Array.prototype.at) {
    Array.prototype.at = function (idx) {
        return (idx >= 0) ? this[idx] : this[this.length + idx];
    }
}

/************************************************************
 * Counts the number of elements that evaluate as true, or,
 * if a function is provided, passes the test implemented by it.
 ************************************************************/
Object.defineProperty(Array.prototype, 'countTrue', {
    value: function(func) {
        var count = 0;
        for (var i = 0; i < this.length; i++) {
            if (i in this
                && (func ? func(this[i], i, this) : this[i])) {
                count++;
            }
        }
        return count;
    }
});

/***********************************************************
 * Given an array of strings, return a string that represents the list
 * as correct English (with Oxford comma, if applicable).
 ***********************************************************/
function englishJoin(list) {
    if (list.length == 0) {
        return '';
    } else {
        /* Given one element, reduce without a starting value returns
           that element */
        return list.reduce(function(str, cur, idx) {
            return str + (list.length > 2 ? ', ' : ' ')
                + (idx == list.length - 1 ? 'and ' : '')
                + cur;
        });
    }
}

/************************************************************
 * Generate a random alphanumeric ID.
 ************************************************************/
function generateRandomID() {
    var ret = ''
    for (let i=0;i<10;i++) {
        ret += 'abcdefghijklmnopqrstuvwxyz1234567890'[getRandomNumber(0,36)]
    }

    return ret;
}

/**
 * Returns a Promise object that resolves (near-)immediately.
 * 
 * This can be used to create "dummy" promises that fill in
 * for actions that are potentially skippable.
 * 
 * @returns {Promise<void>}
 */
function immediatePromise() {
    return new Promise(function (resolve) {
        setTimeout(resolve, 1);
    });
}

/**
 * Fetch an XML resource and parse it.
 * 
 * @param {string} url The resource to fetch.
 * @returns {Promise<jQuery>} The fetched XML contents as a jQuery object.
 */
function fetchXML(url) {
    return fetch(url, { method: "GET" }).then(function (resp) {
        if (resp.status < 200 || resp.status > 299) {
            throw new Error("Fetching " + url + " failed with error " + resp.status + ": " + resp.statusText);
        } else {
            return resp.text();
        }
    }).catch(function() {}).then(function (xml) { return $(xml); });
}

/**
 * Fetch a possibly compressed XML file.
 * Attempts to fetch a compressed version of the file first,
 * then fetches the uncompressed version of the file if that isn't found.
 * 
 * @param {string} url The resource to fetch.
 * @returns {Promise<jQuery>} The fetched and possibly decompressed text contents,
 * parsed as XML using jQuery.
 */
function fetchCompressedURL(baseUrl) {
    return fetch(baseUrl+'.gz', {method: "GET"}).then(function (resp) {
        if (resp.status >= 200 && resp.status <= 299) {
            /* Found compressed data */
            return resp.arrayBuffer().then(function (data) {
                return pako.inflate(new Uint8Array(data), { to: 'string' });
            }).then(function (xml) { return $(xml); });
        } else if (resp.status == 404) {
            /* Fallback to fetching uncompressed */
            return fetchXML(baseUrl);
        } else {
            throw new Error("Fetching " + baseUrl + ".gz failed with error " + resp.status + ": " + resp.statusText);
        }
    });
}

/**
 * POST a JSON object to the given URL.
 * @param {string} url The endpoint to POST to.
 * @param {Object} data The object to stringify and send.
 * @returns {Promise<Response>}
 */
function postJSON(url, data) {
    return fetch(url, {
        'method': 'POST',
        'headers': { 'Content-Type': 'application/json' },
        'body': JSON.stringify(data)
    }).then(function (resp) {
        if (resp.status < 200 || resp.status > 299) {
            throw new Error("POST " + url + " failed with HTTP " + resp.status + " " + resp.statusText);
        }
        return resp;
    });
}


/**********************************************************************
 * Automatically adjusts the size of all font based on screen width.
 **/
function autoResizeFont ()
{
    var w = window.innerWidth, h = window.innerHeight;
    /* resize font. Note: the 3/2 threshold must match the @media CSS block */
    if ($('.wide-screen-container .screen:visible').width() && w / h >= 3/2) {
        // Calculate 4/3 of the height a normal screen would have ((16/9) / (4/3) = 4/3) 
        $(':root').css('font-size', ($('.screen:visible').width() / 4 * 3 / 100)+'px');
    } else if ($('.screen:visible').width()) {
        $(':root').css('font-size', ($('.screen:visible').width() / 100)+'px');
    } else {
        return;
    }

    activeBackground.update();
}

$('button[tabindex], input[tabindex], select[tabindex]').each(function() {
    $(this).data('tabindex', $(this).attr('tabindex'));
});

$('.modal').on('show.bs.modal', function() {
    $('.screen:visible').find('button, input, select').attr('tabindex', -1);
    $(document.activeElement).blur();
});

$('.modal').on('hidden.bs.modal', function() {
    $('.screen:visible').find('button, input, select').each(function() {
        if ($(this).data('tabindex')) {
            $(this).attr('tabindex', $(this).data('tabindex'));
        } else {
            $(this).removeAttr('tabindex');
        }
    });
});
