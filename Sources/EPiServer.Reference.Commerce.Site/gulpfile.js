// Include gulp
var gulp = require('gulp');

// Include LESS/CSS plugins
var less = require('gulp-less'),
    lessAutoprefixer = require('less-plugin-autoprefix'),
    sourceMaps = require('gulp-sourcemaps'),
    cleanCSS = require('gulp-clean-css'),
    rename = require('gulp-rename'),
    plumber = require('gulp-plumber');

var autoprefixer = new lessAutoprefixer({ browsers: ['last 2 versions'] });

// Settings
var inputLESSPartials = './Styles/**/*.less';
var configB2B = {
    inputLESS: './Styles/b2b/global.less',
    outputLESS: './Styles/b2b/',
    outputSourceMaps: './',
    inputMinifier: './Styles/b2b/global.css',
    lessOptions: {
        outputStyle: 'expanded'
    },
    autoprefixerOptions: {
        browsers: ['latest 2 versions']
    }
};


var configQS = {
    inputLESS: './Styles/style.less',
    outputLESS: './Styles/',
    outputSourceMaps: './',
    inputMinifier: './Styles/style.css',
    lessOptions: {
        outputStyle: 'expanded'
    },
    autoprefixerOptions: {
        browsers: ['latest 2 versions']
    }
};

// Compiling less & generate sourcemaps & add vendor prefixes
gulp.task('less', function () {
    return gulp.src([configB2B.inputLESS, configQS.inputLESS], { base: '.' })
        .pipe(plumber({
            errorHandler: onError
        }))
        .pipe(sourceMaps.init())
        .pipe(less({
            plugins: [autoprefixer]
        }))
        .pipe(sourceMaps.write())
        .pipe(gulp.dest('.'));


});

gulp.task('minify', ['less'], function () {
    return gulp.src([configB2B.inputMinifier, configQS.inputMinifier], { base: '.' })
        .pipe(cleanCSS())
        .pipe(rename({
            extname: '.min.css'
        }))
        .pipe(gulp.dest('.', { overwrite: true }));
});

// Watch files for changes
gulp.task('watch', ['less'], function () {
    gulp.watch(inputLESSPartials, ['less']);
});

gulp.task('default', ['watch']);

var onError = function (err) {
    gutil.beep();
    console.log(err);
};