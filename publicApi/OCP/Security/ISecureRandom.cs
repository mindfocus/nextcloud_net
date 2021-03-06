﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Security
{
    /**
     * Class SecureRandom provides a wrapper around the random_int function to generate
     * secure random strings. For PHP 7 the native CSPRNG is used, older versions do
     * use a fallback.
     *
     * Usage:
     * \OC::server.getSecureRandom().generate(10);
     *
     * @package OCP\Security
     * @since 8.0.0
     */
    public interface ISecureRandom
    {



        /**
         * Generate a random string of specified length.
         * @param int length The length of the generated string
         * @param string characters An optional list of characters to use if no character list is
         * 							specified all valid base64 characters are used.
         * @return string
         * @since 8.0.0
         */
        string generate(int length,
                                 string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/");

}

    public class SecureRandomType
    {
        ///**
        // * Flags for characters that can be used for <code>generate(length, characters)</code>
        // */
        public static string CHAR_UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string CHAR_LOWER = "abcdefghijklmnopqrstuvwxyz";
        public static string CHAR_DIGITS = "0123456789";
        public static string CHAR_SYMBOLS = "!\\\"#%&\\\'()* +,-./:;<=>?@[\\]^_`{|}~";

        ///**
        // * Characters that can be used for <code>generate(length, characters)</code>, to
        // * generate human readable random strings. Lower- and upper-case characters and digits 
        // * are included. Characters which are ambiguous are excluded, such as I, l, and 1 and so on.
        // */
        public static string CHAR_HUMAN_READABLE = "abcdefgijkmnopqrstwxyzABCDEFGHJKLMNPQRSTWXYZ23456789";
    }

}
