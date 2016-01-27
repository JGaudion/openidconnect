var gulp = require('gulp');
var sass = require('gulp-sass'); //For compiling sass
var browserRefresh = require('browser-sync'); //For keeping the browser always up to date
var minifyJs = require('gulp-uglify');//For minification of javascript
var useref = require('gulp-useref');//For combining files into one
var minifyCss = require('gulp-cssnano');//For minifying css
var del = require('del');//For removing files that are no longer used.
var sequence = require('run-sequence');//For creating a task where sub-tasks run in a specific order
var gulpIf = require('gulp-if');//To allow if statements

var outputFolder = 'dist';
var contentFolder = 'content';
var sassFiles = contentFolder + '/scss/*.scss';
var cssBuildFolder = contentFolder + '/css';
var cssOutputFolder = outputFolder + '/css';
var htmlFiles = contentFolder + '/**/*.html';


//The perameters in square brackets are functions which should run BEFORE watch... it will start the others first
gulp.task('watchFiles', ['browserRefresh', 'sassToCss'], function () {
    //Watching for changes to sass files
    gulp.watch('content/scss/*.scss', ['sassToCss']);
});

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
        .pipe(gulpIf('*.js', minifyJs({mangle: false})))
        .pipe(gulpIf('*.css', minifyCss()))
        .pipe(gulp.dest(outputFolder));
});

gulp.task('reloadBrowsers', function () {
    return browserRefresh({ server: { baseDir: outputFolder } });
});

gulp.task('buildSite', function (callback) {
    return sequence('clean', 'compileSass', 'buildPage', 'reloadBrowsers', callback);
});

gulp.task('default', ['buildSite']);