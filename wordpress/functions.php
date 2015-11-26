<?php

/* make comment links use #disqus_thread
** https://thomasgriffin.io/change-comment-link-wordpress/
***********************************************************************/
function change_comment_link_for_disqus($link) {
    global $post;
    $hash = '#disqus_thread';
    return get_permalink($post->ID) . $hash;
}

add_filter('get_comments_link', 'change_comment_link_for_disqus', 99);

/* remove comment-reply js (not needed when using disqus)
** http://crunchify.com/try-to-deregister-remove-comment-reply-min-js-jquery-migrate-min-js-and-responsive-menu-js-from-wordpress-if-not-required/
***********************************************************************/
function remove_comment_reply_js(){
    wp_deregister_script('comment-reply');
}

add_action('init', 'remove_comment_reply_js');