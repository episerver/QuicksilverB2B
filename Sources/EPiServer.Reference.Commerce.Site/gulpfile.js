// Include gulp
var gulp = require('gulp');

// Include LESS/CSS plugins
var less = require('gulp-less'),
    lessAutoprefixer = require('less-plugin-autoprefix'),
    sourceMaps = require('gulp-sourcemaps'),
    cleanCSS = require('gulp-clean-css'),
    rename = require('gulp-rename');

var autoprefixer = new lessAutoprefixer({ browsers: ['last 2 versions'] });

// Settings

var config = {
    inputLESS: './Styles/b2b/global.less',
    inputLESSPartials: './Styles/b2b/**/*.less',
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

// Compiling less & generate sourcemaps & add vendor prefixes
gulp.task('less', function () {
    return gulp.src(config.inputLESS)
        .pipe(sourceMaps.init())
        .pipe(less({
			plugins: [autoprefixer]
		}))
        .pipe(sourceMaps.write())
        .pipe(gulp.dest(config.outputLESS));

});

gulp.task('minify', ['less'], function () {
    return gulp.src(config.inputMinifier)
        .pipe(cleanCSS())
        .pipe(rename({
            extname: '.min.css'
        }))
        .pipe(gulp.dest(config.outputLESS));
});

// Watch files for changes
gulp.task('watch', ['less'], function () {
    gulp.watch(config.inputLESSPartials, ['less']);
});

gulp.task('default', ['watch']);
