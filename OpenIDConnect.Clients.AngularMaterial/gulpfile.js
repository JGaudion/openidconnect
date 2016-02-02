'use strict';

var gulp = require('gulp');
var sass = require('gulp-sass');
var minifyJs = require('gulp-uglify');
var useref = require('gulp-useref');
var cssnano = require('gulp-cssnano');
var del = require('del');
var sequence = require('run-sequence');
var gulpIf = require('gulp-if');

var outputFolder = 'dist';
var contentFolder = 'content';
var sassFiles = contentFolder + '/**/*.scss';
var cssOutputFolder = outputFolder + '/css';
var htmlFiles = contentFolder + '/**/*.html';

gulp.task('clean', function () {
    return del([outputFolder, cssBuildFolder]);
});

gulp.task('sass', function(){
    gulp.src(sassFiles)
        .pipe(sass({outputStyle: 'compressed'}).on('error', sass.logError))
        .pipe(cssnano())
        .pipe(gulp.dest(cssOutputFolder));
});

gulp.task('sass:watch', function () {
  gulp.watch(sassFiles, ['sass']);
});

gulp.task('buildPage', function () {
    return gulp.src(htmlFiles)
        .pipe(useref())
        .pipe(gulpIf('*.js', minifyJs({ mangle: false })))
        .pipe(gulp.dest(outputFolder));
});

gulp.task('build', function (callback) {
    return sequence('clean', 'compileSass', 'buildPage', callback);
});

gulp.task('default', ['build', 'watch']);

gulp.task('watch', function () {
    gulp.watch('content/**', ['build']);
});
