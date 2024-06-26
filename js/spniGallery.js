/********************************************************************************
 This file contains the variables and functions that form the gallery screens of
 the game.
 ********************************************************************************/

 /**********************************************************************
  *****                   Gallery Screen UI Elements               *****
  **********************************************************************/

$galleryEndingsScreen = $('#epilogue-gallery-screen');
$galleryCollectiblesScreen = $('#collectible-gallery-screen');
$galleryDecksScreen = $('#deck-gallery-screen');

/**********************************************************************
 *****          Epilogues Gallery Screen UI Elements              *****
 **********************************************************************/
 
$genderTypeButtons = [$('#gallery-gender-all'), $('#gallery-gender-any'), $('#gallery-gender-male'), $('#gallery-gender-female')];
$galleryEndings = $('#gallery-endings-block').children();
$galleryPrevButton = $('#gallery-prev-page-button');
$galleryNextButton = $('#gallery-next-page-button');
$galleryStartButton = $('#gallery-start-ending-button');
$selectedEndingPreview = $('#selected-ending-previev');
$selectedEndingLabels = [$('#selected-ending-title'), $('#selected-ending-character'), $('#selected-ending-gender')];
$selectedEndingHint = [$('#selected-ending-hint-container'), $('#selected-ending-hint')];
$selectedEndingDesc = [$('#selected-ending-description-container'), $('#selected-ending-description')];

/**********************************************************************
 *****          Collectibles Gallery Screen UI Elements           *****
 **********************************************************************/
$collectibleListPane = $('#collectibles-list-pane');
$collectibleImagePane = $('#collectibles-image-pane');
$collectibleTextPane = $('#collectibles-text-pane');

$collectibleTextContainer = $('.collectible-text-container');

$collectibleTitle = $('#collectible-title');
$collectibleSubtitle = $('#collectible-subtitle');
$collectibleCharacter = $('#collectible-character');
$collectibleUnlock = $('#collectible-unlock');
$collectibleProgressContainer = $('#collectible-progress');
$collectibleProgressBar = $('#collectible-progress-bar');
$collectibleProgressText = $('#collectible-progress-text');
$collectibleText = $('#collectible-text');
$collectibleImage = $('#collectible-image');

/**********************************************************************
 *****          Card Decks Gallery Screen UI Elements             *****
 **********************************************************************/
var $deckListPane = $('#deck-list-pane');

var $deckGroupsContainer = $(".deck-cards-container");
var $deckTitle = $("#deck-title");
var $deckSubtitle = $("#deck-subtitle");
var $deckCredits = $("#deck-credits");
var $deckDescription = $("#deck-description");
var $deckStatusAlert = $("#deck-status-alert");

var $deckEnableAllBtn = $("#deck-activate-btn");
var $deckDisableAllBtn = $("#deck-deactivate-btn");


function GEnding(player, ending){
    this.player = player;
    this.gender = $(ending).attr('gender');

    var previewImage = $(ending).attr('img');
    if (previewImage) {
        previewImage = previewImage.charAt(0) === '/' ? previewImage : player.base_folder + previewImage;
    } else {
        console.log("No preview image found for: "+player.id+" ending: "+$(ending).html());
    }
    
    var offlineIndicator = "";
    if ($(ending).attr('status') && $(ending).attr('status') != "online") {
        offlineIndicator = "[Offline] ";
    }

    this.image = previewImage;
    this.rawTitle = $(ending).html()
    this.title = offlineIndicator + $(ending).html();
    this.unlockHint = $(ending).attr('hint');
	this.description = $(ending).attr('description');
    this.unlocked = function() { return EPILOGUES_UNLOCKED || save.hasEnding(player.id, this.rawTitle); };
}

var unescapeSubstitutions = {
    '&lt;': '<',
    '&gt;': '>',
    '&quot;': '"',
    '&apos;': '\'',
    '&amp;': '&',
}
var unescapeDialogueRE = /(&(?:lt|gt|quot|apos|amp);)/gi

function unescapeHTML(in_text) {
    if (!in_text) return '';
    
    return in_text.replace(unescapeDialogueRE, function (match, p1) {
        return unescapeSubstitutions[p1];
    });
}

function Collectible(xmlElem, player) {
    this.id = xmlElem.attr('id');
    this.image = xmlElem.attr('img');
    this.thumbnail = xmlElem.attr('thumbnail') || this.image;
    this.status = xmlElem.attr('status');
    this.title = unescapeHTML(xmlElem.children('title').text());
    this.subtitle = unescapeHTML(xmlElem.children('subtitle').text());
    this.unlock_hint = unescapeHTML(xmlElem.children('unlock').text());
    this.text = unescapeHTML(xmlElem.children('text').html());
    this.detailsHidden = xmlElem.children('hide-details').text() === 'true';
    this.hidden = xmlElem.children('hidden').text() === 'true';
    this.counter = parseInt(xmlElem.children('counter').text(), 10) || undefined;
    
    if (this.counter <= 0) this.counter = undefined;
    
    if (player) {
        this.source = player.metaLabel;
        this.player = player;
    } else {
        this.source = 'The Inventory';
        this.player = undefined;
    }

    this.clothing = null;
    
    var clothingElems = xmlElem.children("clothing").map(function () { return $(this); }).get();
    if (clothingElems.length > 0) {
        var $elem = clothingElems[0];
        var generic = $elem.attr('generic');
        var name = $elem.attr('name');
        var type = $elem.attr('type');
        var position = $elem.attr('position');
        var plural = $elem.attr('plural');
        plural = (plural == 'null' ? null : plural == 'true');

        var genders = $elem.attr('gender') || "all";
        var image = $elem.attr('img') || this.image;

        var newClothing = new PlayerClothing(name, generic, type, position, image, plural, this.id, genders, this);
        this.clothing = newClothing;

        if (!this.status || includedOpponentStatuses[this.status]) {
            PLAYER_CLOTHING_OPTIONS[newClothing.id] = newClothing;
        }
    }
}

Collectible.prototype.isUnlocked = function (ignoreUnlockAllOption) {
    if (COLLECTIBLES_UNLOCKED && !ignoreUnlockAllOption) return true;
    
    var curCounter = save.getCollectibleCounter(this);
    if (this.counter) {
        return curCounter >= this.counter;
    } else {
        return curCounter > 0;
    }
}

Collectible.prototype.getCounter = function () {
    var ctr = save.getCollectibleCounter(this);
    return (this.counter && ctr > this.counter) ? this.counter : ctr;
}

Collectible.prototype.unlock = function () {
    save.setCollectibleCounter(this, this.counter || 1);
}

Collectible.prototype.incrementCounter = function (inc) {
    var newCounter = save.getCollectibleCounter(this) + inc;
    
    if (this.counter && newCounter > this.counter)
        newCounter = this.counter;
    
    save.setCollectibleCounter(this, newCounter); 
}

Collectible.prototype.setCounter = function (val) {
    if (this.counter && val > this.counter)
        val = this.counter;
        
    save.setCollectibleCounter(this, val); 
}

Collectible.prototype.display = function () {
    var offlineIndicator = "";
    if (this.status && this.status != "online") {
        offlineIndicator = "[Offline] ";
    }
    
    if ((!this.detailsHidden && !this.hidden) || this.isUnlocked()) {
        $collectibleTitle.html(offlineIndicator + this.title);
        $collectibleSubtitle.html(this.subtitle).show();
    } else {
        $collectibleTitle.html(offlineIndicator + "[Locked]");
        $collectibleSubtitle.html("").hide();
    }
    
    $collectibleCharacter.text(this.source);
    $collectibleUnlock.html(this.unlock_hint);
    
    if (this.counter) {
        var curCounter = this.getCounter();
        var pct = Math.round((curCounter / this.counter) * 100);
        
        $collectibleProgressBar
            .attr('aria-valuenow', pct)
            .css('width', pct+'%');
        $collectibleProgressContainer.show();
        
        $collectibleProgressText.text('('+curCounter+' / '+this.counter+')').show();
    } else {
        $collectibleProgressContainer.hide();
        $collectibleProgressText.hide();
    }
    
    $collectibleTextPane.show();
    
    if (this.isUnlocked()) {
        $collectibleText.html(this.text);
        $collectibleTextContainer.show();
        
        if (this.image) {
            $collectibleImage.attr('src', this.image);
            $collectibleImagePane.show();
        } else {
            $collectibleImagePane.hide();
        }
    } else {
        $collectibleTextContainer.hide();
        $collectibleImagePane.hide();
    }
};

Collectible.prototype.listElement = function () {
    if (this.status && !includedOpponentStatuses[this.status]) {
        return null;
    }
    
    if (this.hidden && !this.isUnlocked()) {
        return null;
    }
    
    var baseElem = $('<div class="gallery-pane-list-item bordered"></div>');
    var imgElem = $('<img class="gallery-pane-item-icon">');
    var titleElem = $('<div class="gallery-pane-item-title"></div>');
    var subtitleElem = $('<div class="gallery-pane-item-subtitle"></div>');
    
    var offlineIndicator = "";
    if (this.status && this.status != "online") {
        offlineIndicator = "[Offline] ";
    }
    
    if (!this.detailsHidden || this.isUnlocked()) {
        titleElem.html(offlineIndicator + this.title);
        subtitleElem.html(this.subtitle);
    } else {
        titleElem.html(offlineIndicator + "[Locked]");
        subtitleElem.html(this.unlock_hint);
    }
    
    if (this.counter) {
        var curCounter = this.getCounter();
        var curSubtitle = subtitleElem.html();
        subtitleElem.html(curSubtitle + ' ('+curCounter+' / '+this.counter+')');
    }
    
    if (this.isUnlocked()) {
        imgElem.attr('src', this.thumbnail);
    } else {
        imgElem.attr('src', "img/unknown.svg");
    }
    
    baseElem.append(imgElem, titleElem, subtitleElem).click(this.display.bind(this));
    return baseElem;
};

Collectible.prototype.displayInfoModal = function () {
    $('#collectible-info-thumbnail').attr('src', this.thumbnail);
    $('#collectible-info-title').html(this.title);
    $('#collectible-info-subtitle').html(this.subtitle);
    
    $collectibleInfoModal.modal('show');
    
    /* Hide the modal if the user clicks anywhere outside of it. */
    $('.modal-backdrop').one('click', function () {
        $collectibleInfoModal.modal('hide');
    })
}

/**********************************************************************
 *****                   Gallery Screen UI Functions              *****
 **********************************************************************/
 
 
/* opponent listing file */
var galleryEndings = [];
var allEndings = [];
var galleryPage = 0;
var galleryPages = -1;
var epp = 20;
var selectedEnding = -1;
var GALLERY_GENDER = 'all';

var playerCollectibles = {}; /* Indexed by player ID. */

/** @type {CardDeckDisplay?} */
var currentDeckDisplay = null;

function goToEpiloguesScreen() {
    Sentry.setTag("screen", "gallery-epilogues");

    $galleryEndingsScreen.show();
    $galleryCollectiblesScreen.hide();
    $galleryDecksScreen.hide();
    
    loadGalleryEndings();
    updateGalleryScreen();
}

function goToCollectiblesScreen() {
    Sentry.setTag("screen", "gallery-collectibles");

    $galleryCollectiblesScreen.show();
    $galleryEndingsScreen.hide();
    $galleryDecksScreen.hide();
    updateCollectiblesScreen();
    
    $collectibleTextPane.hide();    
    $collectibleImagePane.hide();    
}

function goToCardsScreen() {
    Sentry.setTag("screen", "gallery-decks");

    if (!currentDeckDisplay) {
        currentDeckDisplay = new CardDeckDisplay(CARD_IMAGE_SETS[DEFAULT_CARD_DECK]);
    }
    currentDeckDisplay.render();

    $galleryDecksScreen.show();
    $galleryCollectiblesScreen.hide();
    $galleryEndingsScreen.hide();
}

function createFilterOption (opp) {
    var elem = document.createElement('option');
    elem.value = opp.id;
    elem.text = opp.metaLabel;
    elem.className = 'gallery-character-filter-option'
    
    return elem;
}

function loadGalleryScreen(){
    screenTransition($titleScreen, $galleryScreen);

    if (!CARD_DECKS_ENABLED) {
        $(".deck-switch-button").hide();
    } else {
        $deckListPane.empty().append(Object.values(CARD_IMAGE_SETS).filter(function (set) {
            return set.isAvailable();
        }).map(createDeckListElement));
    }
    
    /* Set up filter lists: */
    
    // Clear all previously populated list items:
    $('.gallery-character-filter-option').detach();
    
    $('#collectible-character-filter').append(loadedOpponents.filter(function (opp) {
        return opp && opp.has_collectibles;
    }).sort(function(a, b) { return a.metaLabel < b.metaLabel ? -1 : 1; }).map(createFilterOption));
    
    $('#epilogue-character-filter').append(loadedOpponents.filter(function (opp) {
        return opp && opp.endings;
    }).sort(function(a, b) { return a.metaLabel < b.metaLabel ? -1 : 1; }).map(createFilterOption));
    
    if (COLLECTIBLES_ENABLED) {
        goToCollectiblesScreen();
        if (!EPILOGUES_ENABLED) {
            $('.epilogues-switch-button').hide();
        }
    } else {
        goToEpiloguesScreen();
        $('.collectibles-switch-button').hide();
    }
}

function backGalleryScreen(){
    Sentry.setTag("screen", "title");
    screenTransition($galleryScreen, $titleScreen);
}

function changeCharacterFilter (collectibleScreen) {
    if (collectibleScreen) {
        updateCollectiblesScreen();
    } else {
        updateGalleryScreen();
    }
}

function loadGeneralCollectibles () {
    return metadataIndex.getFile('opponents/general_collectibles.xml').then(function($xml) {
        $xml.children('collectible').each(function (idx, elem) {
            generalCollectibles.push(new Collectible($(elem), undefined));
        });
    }).catch(function (err) {
        console.error("Failed to load general collectibles");
        captureError(err);
    });
}

function loadAllCollectibles() {
    console.log("Loading all collectibles");
    return loadGeneralCollectibles().then(function () {
        beginStartupStage("Collectibles");

        var nLoaded = 0;
        return Promise.all(loadedOpponents.map(function (opp) {
            var ret = opp ? opp.fetchCollectibles() : immediatePromise();
            return ret.then(function () {
                updateStartupStageProgress(++nLoaded, loadedOpponents.length);
            });
        }));
    });
}

function updateCollectiblesScreen() {    
    $collectibleListPane.empty();
    
    var filter = $('#collectible-character-filter').val();
    var showLocked = filter && filter === '__locked' 
    
    if (!filter || filter === '__general' || showLocked) {
        generalCollectibles.forEach(function (item) {
            if (showLocked && item.isUnlocked()) return;

            var elem = item.listElement();
            if (elem) {
                $collectibleListPane.append(elem);    
            }
        });
    }
    
    loadedOpponents.forEach(function (opp) {
        if (!opp) return;

        if (opp.collectibles) {
            if (!opp.has_collectibles) {
                $('#collectible-character-filter [value=\"'+opp.id+'\"]').remove();
                return;
            }

            if (filter && !showLocked && opp.id !== filter) return;
            
            opp.collectibles.forEach(function (item) {
                if (showLocked && item.isUnlocked()) return;

                var elem = item.listElement();
                if (elem) {
                    $collectibleListPane.append(elem);    
                }
            });
        }
    });
}


function loadGalleryEndings(){
    if(allEndings.length > 0){
        return;
    }
    
    for(var i=0; i<loadedOpponents.length; i++){
        if (loadedOpponents[i] && loadedOpponents[i].endings) {
            loadedOpponents[i].endings.each(function () {
                var status = $(this).attr('status');
                if (status && !includedOpponentStatuses[status]) {
                    return;
                }

                var gending = new GEnding(loadedOpponents[i], this);
                allEndings.push(gending);
            });
        }
    }
}
function updateGalleryScreen () {
    var charFilter = $('#epilogue-character-filter').val();
    
    galleryEndings = allEndings.filter(function (ending) {
        if (charFilter && ending.player.id !== charFilter) return false;
        
        switch (GALLERY_GENDER) {
        case 'male':
            if (ending.gender !== 'male') return false;
            break;
        case 'female':
            if (ending.gender !== 'female') return false;
            break;
        case 'any':
            if (ending.gender === 'male' || ending.gender === 'female') return false;
            break;
        default:
            break;
        }
        
        return true;
    });
    
    galleryPages = Math.ceil(galleryEndings.length/parseFloat(epp));
    galleryPage = 0;
    $galleryPrevButton.attr('disabled', true);
    if(galleryPages==1){
        $galleryNextButton.attr('disabled', true);
    }
    else{
        $galleryNextButton.attr('disabled', false);
    }
    loadThumbnails();
}

function loadEndingThunbnail(element, ending){
    element.removeClass('empty-thumbnail');
    if (ending.unlocked()) {
        element.removeClass('unlocked-thumbnail');
        element.css('background-image','url(\''+ending.image+'\')');
    } else {
        element.css('background-image', '');
        element.addClass('unlocked-thumbnail');
    }
}

function loadThumbnails(){
    var i=0;
    for(; i<epp && epp*galleryPage+i<galleryEndings.length; i++){
        loadEndingThunbnail($galleryEndings.eq(i), galleryEndings[epp*galleryPage+i]);
    }
    for( ; i<epp; i++){
        $galleryEndings.eq(i).removeClass('unlocked-thumbnail');
        $galleryEndings.eq(i).addClass('empty-thumbnail');
        $galleryEndings.eq(i).css('background-image', 'none');
    }
}


function galleryGender(gender){
    GALLERY_GENDER = gender;
    $('.gallery-gender-button').css('opacity', 0.4);
    switch(gender){
        case 'male':
            $('#gallery-gender-male').css('opacity', 1);
            break;
        case 'female':
            $('#gallery-gender-female').css('opacity', 1);
            break;
        case 'any':
            $('#gallery-gender-any').css('opacity', 1);
            break;
        default:
            $('#gallery-gender-all').css('opacity', 1);
            break;
    }
    
    updateGalleryScreen();
}

function galleryNextPage(){
    galleryPage++;
    loadThumbnails();
    $galleryEndings.css('opacity', '');
    $galleryPrevButton.attr('disabled', false);
    if(galleryPage+1==galleryPages){
        $galleryNextButton.attr('disabled', true);
    }
}

function galleryPrevPage(){
    galleryPage--;
    loadThumbnails();
    $galleryEndings.css('opacity', '');
    $galleryNextButton.attr('disabled', false);
    if(galleryPage==0){
        $galleryPrevButton.attr('disabled', true);
    }
}

function selectEnding(i) {
    selectedEnding = epp*galleryPage+i;
    var ending = galleryEndings[selectedEnding];

    if (!ending) {
        return;
    }
    
    if (ending.unlockHint) {
        $selectedEndingHint[0].show();
        $selectedEndingHint[1].html(ending.unlockHint);
    } else {
        $selectedEndingHint[0].hide();
    }

    if (ending.description) {
        $selectedEndingDesc[0].show();
        $selectedEndingDesc[1].html(ending.description);
    } else {
        $selectedEndingDesc[0].hide();
    }

    if (ending.unlocked()) {
        $galleryStartButton.attr('disabled', false);
        $selectedEndingLabels[0].html(ending.title);
    } else {
        $galleryStartButton.attr('disabled', true);
        $selectedEndingLabels[0].html('');
    }
    
    $galleryEndings.css('opacity', '');
    $galleryEndings.eq(i).css('opacity', 1);
    loadEndingThunbnail($selectedEndingPreview, ending);
    $selectedEndingLabels[1].html(ending.player.metaLabel);
    $selectedEndingLabels[2].html(ending.gender);
    switch(ending.gender){
        case 'male':
            $selectedEndingLabels[2].removeClass('female-style');
            $selectedEndingLabels[2].addClass('male-style');
            break;
        case 'female':
            $selectedEndingLabels[2].removeClass('male-style');
            $selectedEndingLabels[2].addClass('female-style');
            break;
        default:
            $selectedEndingLabels[2].removeClass('female-style');
            $selectedEndingLabels[2].removeClass('male-style');
    }
}

function doEpilogueFromGallery(){
    var epilogue;
    if (!selectedEnding < 0 || !(epilogue = galleryEndings[selectedEnding])) {
        return;
    }

    var player = epilogue.player;
    $galleryStartButton.attr('disabled', true);
    
    player.fetchBehavior()
        /* Success callback.
         * 'this' is bound to the Opponent object.
         */
        .then(function($xml) {
            var endingElem = null;
            
            $xml.children('epilogue').each(function () {
                console.log($(this).children('title').html());
                if ($(this).children('title').html() === epilogue.rawTitle && $(this).attr('gender') === epilogue.gender) {
                    endingElem = this;
                }
            });

            player.loadStylesheet();

            if($nameField.val()){
                humanPlayer.label = $nameField.val();
            } else {
                switch(epilogue.gender){
                    case "male": humanPlayer.label = "Mister"; break;
                    case "female" : humanPlayer.label = "Miss"; break;
                    default: humanPlayer.label = (humanPlayer.gender=="male")?"Mister":"Miss";
                }
            }
            
            // function definition in spniEpilogue.js
            epilogue = parseEpilogue(player, endingElem);

            /* Load forward-declarations for persistent markers. */
            $xml.find('persistent-markers>marker').each(function (i, elem) {
                var markerName = $(elem).text();
                player.persistentMarkers[markerName] = true;
            });

            /* Execute marker operations. */
            epilogue.markers.forEach(function(markerOp) {
                if (markerOp.from_gallery) {
                    markerOp.apply(player, null);
                }
            });
        
            recordEpilogueEvent(true, epilogue);
            loadEpilogue(epilogue, null, true); //initialise buttons and text boxes
            screenTransition($galleryScreen, $epilogueScreen);
            $galleryStartButton.attr('disabled', false);
        });
}

/**********************************************************************
 *****          Card Decks Gallery Screen Functions               *****
 **********************************************************************/

 /**
  * Base class for custom card deck selectors.
  * @param {CardDeckGroup} parent
  * @param {CardImageSet} imageSet 
  */
function CardSelector (parent, imageSet) {
    this.parent = parent;
    this.imageSet = imageSet;
    this.elem = createElementWithClass("img", "bordered custom-deck-image");
    $(this.elem).attr({
        src: this.image(),
        alt: this.altText()
    }).click(this.onClick.bind(this));

    this.update();
}

CardSelector.prototype.image = function () { return ""; }
CardSelector.prototype.altText = function () { return ""; }
CardSelector.prototype.isSelected = function () { return false; }
CardSelector.prototype.isUnlocked = function () { return this.imageSet.isUnlocked(); }
CardSelector.prototype.select = function () {}
CardSelector.prototype.deselect = function () {}

CardSelector.prototype.update = function () {
    var $elem = $(this.elem);

    $elem.removeClass('usable selected');
    this.isUnlocked().then(function (unlocked) {
        if (unlocked) {
            $elem.addClass('usable');
            if (this.isSelected()) $elem.addClass('selected');
        }
    }.bind(this));
}

CardSelector.prototype.onClick = function () {
    this.isUnlocked().then(function (unlocked) {
        if (unlocked) {
            if (!this.isSelected()) {
                this.select();
            } else {
                this.deselect();
            }
            this.parent.updateEnableDisableButtons();
            ACTIVE_CARD_IMAGES.save();
        }
    }.bind(this));
}

/**
 * A UI element for selecting card fronts.
 * 
 * @param {CardDeckGroup} parent
 * @param {CardImageSet} imageSet 
 * @param {Card} card
 */
function CardFrontSelector (parent, imageSet, card) {
    this.card = card;
    CardSelector.call(this, parent, imageSet);
}

CardFrontSelector.prototype = Object.create(CardSelector.prototype);
CardFrontSelector.prototype.constructor = CardFrontSelector;

CardFrontSelector.prototype.image = function () {
    return this.imageSet.frontImages[this.card.toString()];
}

CardFrontSelector.prototype.altText = function () {
    return this.card.altText();
}

CardFrontSelector.prototype.isSelected = function () {
    return ACTIVE_CARD_IMAGES.isFrontImageActive(this.imageSet, this.card);
}

CardFrontSelector.prototype.select = function () {
    ACTIVE_CARD_IMAGES.activateFrontImage(this.imageSet, this.card);
    this.update();
}

CardFrontSelector.prototype.deselect = function () {
    ACTIVE_CARD_IMAGES.deactivateFrontImage(this.card);
    this.update();
}

/**
 * A UI element for selecting card backs.
 * 
 * @param {CardDeckGroup} parent
 * @param {CardImageSet} imageSet 
 * @param {string} image
 */
function CardBackSelector (parent, imageSet, image) {
    this.imgID = image;
    CardSelector.call(this, parent, imageSet);
}

CardBackSelector.prototype = Object.create(CardSelector.prototype);
CardBackSelector.prototype.constructor = CardBackSelector;

CardBackSelector.prototype.image = function () {
    return this.imageSet.backImages[this.imgID];
}

CardBackSelector.prototype.altText = function () {
    return "card back";
}

CardBackSelector.prototype.isSelected = function () {
    return ACTIVE_CARD_IMAGES.isBackImageActive(this.imageSet, this.imgID);
}

CardBackSelector.prototype.select = function () {
    ACTIVE_CARD_IMAGES.addBackImage(this.imageSet, this.imgID);
    this.update();
}

CardBackSelector.prototype.deselect = function () {
    ACTIVE_CARD_IMAGES.removeBackImage(this.imageSet, this.imgID);
    this.update();
}

/**
 * An element containing multiple cards, used for grouping together cards of
 * a given suit or for grouping together card back images.
 * 
 * @param {CardDeckDisplay} parent
 * @param {string} title
 * @param {CardImageSet} imageSet
 * @param {Array<Card | string>} cards 
 * @param {boolean} cardBacks
 */
function CardDeckGroup (parent, title, imageSet, cards, cardBacks) {
    this.parent = parent;
    this.imageSet = imageSet;
    this.cardBacks = cardBacks;

    this.mainContainer = createElementWithClass("div", "deck-rank-container");

    var titleContainer = this.mainContainer.appendChild(createElementWithClass("div", "rank-title-container"));

    var titleElem = titleContainer.appendChild(createElementWithClass("span", "deck-rank-title"));
    $(titleElem).text(title);
    
    this.disableAllButton = $(titleContainer.appendChild(createElementWithClass(
        "button",
        "rank-deactivate-btn rank-activation-btn bordered smooth-button red"
    )));
    this.disableAllButton.text("Disable All").click(this.disableAll.bind(this)).hide();
    this.disableBtnShown = false;

    this.enableAllButton = $(titleContainer.appendChild(createElementWithClass(
        "button",
        "rank-activate-btn rank-activation-btn bordered smooth-button green"
    )));
    this.enableAllButton.text("Enable All").click(this.enableAll.bind(this)).hide();
    this.enableBtnShown = false;

    this.cardContainer = this.mainContainer.appendChild(createElementWithClass("div", "rank-cards-container"));

    /** @type {Array<CardSelector>} */
    this.selectors = cards.map(function (card) {
        var selector = null;
        if (!cardBacks) {
            selector = new CardFrontSelector(this, imageSet, card);
        } else {
            selector = new CardBackSelector(this, imageSet, card);
        }
        this.cardContainer.appendChild(selector.elem);
        return selector;
    }.bind(this));
}

CardDeckGroup.prototype.update = function () {
    this.selectors.forEach(function (selector) {
        selector.update();
    });

    this.updateEnableDisableButtons();
}

CardDeckGroup.prototype.updateEnableDisableButtons = function () {
    this.imageSet.isUnlocked().then(function (unlocked) {
        if (unlocked) {
            let allSelected = true;
            let allDeselected = true;
            this.selectors.forEach(function (selector) {
                var s = selector.isSelected();
                allSelected = allSelected && s;
                allDeselected = allDeselected && !s;
            });
        
            let prevEnableState = this.enableBtnShown;
            let prevDisableState = this.disableBtnShown;
        
            if (allSelected) {
                this.enableAllButton.hide();
                this.enableBtnShown = false;
            } else {
                this.enableAllButton.show();
                this.enableBtnShown = true;
            }
        
            /* Regarding the second part of this conditional:
             * - The front images for the default set can only be deactivated by activating
             * front images from other sets.
             * - However, the back images for the default set can be deactivated like
             * images from any other set, UNLESS the default back images are the only
             * ones activated.
             */
            if (
                allDeselected || 
                (this.imageSet.id === DEFAULT_CARD_DECK && (!this.cardBacks || !ACTIVE_CARD_IMAGES.backImages))
            ) {
                this.disableAllButton.hide();
                this.disableBtnShown = false;
            } else {
                this.disableAllButton.show();
                this.disableBtnShown = true;
            }
        
            if (
                (this.enableBtnShown !== prevEnableState) ||
                (this.disableBtnShown !== prevDisableState)
            ) {
                this.parent.updateEnableDisableButtons();
            }
        } else {
            this.enableAllButton.hide();
            this.disableAllButton.hide();
            this.enableBtnShown = false;
            this.disableBtnShown = false;
        }
    }.bind(this));
}

CardDeckGroup.prototype.enableAll = function () {
    this.selectors.forEach(function (selector) {
        selector.select();
    });

    this.updateEnableDisableButtons();
    ACTIVE_CARD_IMAGES.save();
}

CardDeckGroup.prototype.disableAll = function () {
    this.selectors.forEach(function (selector) {
        selector.deselect();
    });

    this.updateEnableDisableButtons();
    ACTIVE_CARD_IMAGES.save();
}

/**
 * 
 * @param {CardImageSet} imageSet 
 */
function CardDeckDisplay (imageSet) {
    this.imageSet = imageSet;

    var suits = [[], [], [], []];
    imageSet.includedFrontCards.forEach(function (c) {
        suits[c.suit].push(c);
    });

    /** @type {Array<CardDeckGroup>} */
    this.groups = [];
    suits.forEach(function (suit, idx) {
        if (suit.length === 0) return;

        suit.sort(function (a, b) {
            return a.rank - b.rank;
        });

        this.groups.push(new CardDeckGroup(
            this, cardSuitToString(idx), imageSet, suit, false
        ));
    }.bind(this));

    var backKeys = Object.keys(imageSet.backImages);
    if (backKeys.length > 0) {
        this.groups.push(new CardDeckGroup(
            this, "Card Backs", imageSet, backKeys, true
        ));
    }
}

CardDeckDisplay.prototype.render = function () {
    $deckGroupsContainer.empty().append(this.groups.map(function (g) {
        return g.mainContainer;
    }));

    $deckTitle.text(this.imageSet.title);

    if (this.imageSet.subtitle) {
        $deckSubtitle.html(this.imageSet.cresubtitledits).show();
    } else {
        $deckSubtitle.hide();
    }

    if (this.imageSet.credits) {
        $deckCredits.html(this.imageSet.credits).show();
    } else {
        $deckCredits.hide();
    }

    if (this.imageSet.description) {
        $deckDescription.text(this.imageSet.description);
    } else {
        $deckDescription.text("<no description provided>");
    }

    $deckStatusAlert.removeClass('locked').addClass('loading').text("(Loading...)").show();

    this.groups.forEach(function (group) {
        group.update();
    });

    this.imageSet.isUnlocked().then(function (unlocked) {
        if (unlocked) {
            $deckStatusAlert.hide();
        } else {
            $deckStatusAlert.addClass("locked").removeClass("loading");

            if (!this.imageSet.isAvailable()) {
                $deckStatusAlert.text("(This deck is not available for use.)").show();
            } else {
                $deckStatusAlert.text("(You haven't unlocked this deck yet.)").show();
            }
        }
    }.bind(this));
    
    $deckEnableAllBtn.off("click").click(this.enableAll.bind(this));
    $deckDisableAllBtn.off("click").click(this.disableAll.bind(this));
    this.updateEnableDisableButtons();
}

CardDeckDisplay.prototype.updateEnableDisableButtons = function () {
    this.imageSet.isUnlocked().then(function (unlocked) {
        if (unlocked) {
            let anyEnableShown = false;
            let anyDisableShown = false;
            this.groups.forEach(function (group) {
                anyEnableShown = anyEnableShown || group.enableBtnShown;
                anyDisableShown = anyDisableShown || group.disableBtnShown;
            });
        
            if (anyEnableShown) {
                $deckEnableAllBtn.show();
            } else {
                $deckEnableAllBtn.hide();
            }
        
            if (anyDisableShown) {
                $deckDisableAllBtn.show();
            } else {
                $deckDisableAllBtn.hide();
            }
        } else {
            $deckEnableAllBtn.hide();
            $deckDisableAllBtn.hide();
        }
    }.bind(this));
}

CardDeckDisplay.prototype.enableAll = function () {
    this.groups.forEach(function (group) {
        group.enableAll();
    });

    this.updateEnableDisableButtons();
}

CardDeckDisplay.prototype.disableAll = function () {
    this.groups.forEach(function (group) {
        group.disableAll();
    });

    this.updateEnableDisableButtons();
}

/**
 * Display a CardImageSet in the Card Decks view.
 * @param {CardImageSet} imageSet 
 */
function displayCardDeck (imageSet) {
    if (!currentDeckDisplay || currentDeckDisplay.imageSet !== imageSet) {
        currentDeckDisplay = new CardDeckDisplay(imageSet);
    }
    currentDeckDisplay.render();
}

/**
 * Create a list element for the left-hand panel of the Card Decks view.
 * @param {CardImageSet} imageSet 
 */
function createDeckListElement (imageSet) {
    var baseElem = createElementWithClass("div", "gallery-pane-list-item deck-list-item bordered");
    var titleElem = createElementWithClass("div", "gallery-pane-item-title");
    var subtitleElem = createElementWithClass("div", "gallery-pane-item-subtitle");
    
    $(titleElem).html(imageSet.title);
    $(subtitleElem).html(imageSet.subtitle);
    $(baseElem).append(titleElem, subtitleElem).click(displayCardDeck.bind(null, imageSet));

    return baseElem;
}
