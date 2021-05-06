/* Theme Name: Zairo - Responsive bootsrap 4 Landing Page Template
   Author: Coderthemes
   Version: 1.0.0
   Created: May 2018
   File Description: App layout
*/


(function ($) {

    "use strict";

    // Screenshot Slider
    $("#owl-demo-screenshot").owlCarousel({
        autoPlay: 3000,
        items: 4,
        itemsDesktop: [1199, 3],
        itemsDesktopSmall: [979, 3]

    });

    // Magnific Popup
    $('.mfp-image').magnificPopup({
        type: 'image',
        closeOnContentClick: true,
        mainClass: 'mfp-fade',
        gallery: {
            enabled: true,
            navigateByImgClick: true,
            preload: [0, 1]
        }
    });

    //Testimonials Slider
    $("#owl-demo").owlCarousel({
        autoPlay: 3000,
        navigation: false,
        slideSpeed: 300,
        paginationSpeed: 400,
        singleItem: true
    });

})(jQuery);