var gulp = require('gulp');
var sass = require('gulp-sass');
var minifyJs = require('gulp-uglify');
var useref = require('gulp-useref');
var minifyCss = require('gulp-cssnano');
var del = require('del');
var sequence = require('run-sequence');
var gulpIf = require('gulp-if');

var outputFolder = 'dist';
var contentFolder = 'content';
var sassFiles = contentFolder + '/scss/*.scss';
var cssBuildFolder = contentFolder + '/css';
var cssOutputFolder = outputFolder + '/css';
var htmlFiles = contentFolder + '/**/*.html';

gulp.task('clean', function () {
    return del([outputFolder, cssBuildFolder]);
});

gulp.task('compileSass', function () {
    return gulp.src(sassFiles)
        .pipe(sass())
        .pipe(gulp.dest(cssBuildFolder));
});

gulp.task('buildPage', function () {
    return gulp.src(htmlFiles)
        .pipe(useref())
        .pipe(gulpIf('*.js', minifyJs({ mangle: false })))
        .pipe(gulpIf('*.css', minifyCss()))
        .pipe(gulp.dest(outputFolder));
});

gulp.task('build', function (callback) {
    return sequence('clean', 'compileSass', 'buildPage', callback);
});

gulp.task('default', ['build', 'watch']);

gulp.task('watch', function () {
    gulp.watch('content/**', ['build']);
});