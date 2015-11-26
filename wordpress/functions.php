<?php

/* retain social proof for older posts
** http://soderlind.no/wordpress-hook-into-another-hook/
***********************************************************************/
function get_social_permalink($permalink, $post, $leavename) {
    // only run when get_permalink is called from get_icon_output (within genesis simple share plugin)
    if (function_exists('wp_debug_backtrace_summary') && stristr(wp_debug_backtrace_summary(), 'get_icon_output') !== FALSE) {
        $url_change_date = strtotime('04/11/2015'); // use date you changed permalink structure
        $post_date = strtotime(get_the_date());

        if ($post_date < $url_change_date) {
            $url_date_prefix = sprintf("/%s/%s/%s", date('Y', $post_date),  date('m', $post_date), date('d', $post_date));
            $permalink = str_replace(site_url(), site_url() . $url_date_prefix, $permalink);
        }
    }
    return $permalink;
}

add_filter( 'post_link','get_social_permalink', 20, 3 );

/* make comment links use #disqus_thread
** https://thomasgriffin.io/change-comment-link-wordpress/
***********************************************************************/
function change_comment_link_for_disqus($link) {
    global $post;
    $hash = '#disqus_thread';
    return get_permalink($post->ID) . $hash;
}

add_filter('get_comments_link', 'change_comment_link_for_disqus', 99);

/* remove query string version numbers from css/js
** https://rtcamp.com/tutorials/wordpress/remove-query-string-css-js-files/
***********************************************************************/
function remove_query_strings_from_js() {
    global $wp_scripts;
    if (!is_a($wp_scripts, 'WP_Scripts')) {
        return;
    }

    foreach ($wp_scripts->registered as $handle => $script) {
        $wp_scripts->registered[$handle]->ver = null;
    }
}

function remove_query_strings_from_css() {
    global $wp_styles;

    if (!is_a($wp_styles, 'WP_Styles')) {
        return;
    }

    foreach ($wp_styles->registered as $handle => $style) {
        $wp_styles->registered[$handle]->ver = null;
    }
}

add_action('wp_print_scripts', 'remove_query_strings_from_js', 999);
add_action('wp_print_footer_scripts', 'remove_query_strings_from_js', 999);

add_action('admin_print_styles', 'remove_query_strings_from_css', 999);
add_action('wp_print_styles', 'remove_query_strings_from_css', 999);

/* remove comment-reply js (not needed when using disqus)
** http://crunchify.com/try-to-deregister-remove-comment-reply-min-js-jquery-migrate-min-js-and-responsive-menu-js-from-wordpress-if-not-required/
***********************************************************************/
function remove_comment_reply_js(){
    wp_deregister_script('comment-reply');
}

add_action('init', 'remove_comment_reply_js');