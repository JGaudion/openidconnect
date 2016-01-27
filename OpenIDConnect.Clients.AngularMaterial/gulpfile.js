var gulp = require('gulp');
var sass = require('gulp-sass'); //For compiling sass
var browserRefresh = require('browser-sync'); //For keeping the browser always up to date
var minifyJs = require('gulp-uglify');//For minification of javascript
var useref = require('gulp-useref');//For combining files into one
var minifyCss = require('gulp-cssnano');//For minifying css
var del = require('del');//For removing files that are no longer used.
var sequence = require('run-sequence');//For creating a task where sub-tasks run in a specific order
var gulpIf = require('gulp-if');//To allow if statements

//The perameters in square brackets are functions which should run BEFORE watch... it will start the others first
gulp.task('watchFiles', ['browserRefresh', 'sassToCss'], function () {
    //Watching for changes to sass files
    gulp.watch('content/scss/*.scss', ['sassToCss']);
});

//Update the browser automatically when there are changes
gulp.task('browserRefresh', function () {
    browserRefresh({
        server: {
            baseDir: 'content'  //lets browser refresh know where the root of the server is
        }
    });
});

//Convert sass files to css
gulp.task('sassToCss', function () {
    return gulp.src('content/scss/*.scss')
        .pipe(sass()) //using gulp-sass
        .pipe(gulp.dest('content/css'))
        //Reload the browser with the converted changes
        .pipe(browserRefresh.reload({
            stream: true
        }));
});

gulp.task('combo', function () {
    return gulp.src('content/*.html')
        .pipe(useref())//using the gulp-useref to combine everything (it uses the comment tags in the html pages)
    //Find the javascript files and minify them
    .pipe(gulpIf('*.js', minifyJs())) //An IF statement in gulp
    //Find the css files and minify them
    .pipe(gulpIf('*.css', minifyCss()))
    .pipe(gulp.dest('dist'));
});
//All the scripts within the <!--build tags in the html pages will be turned into a single javascript file that the html will then point to

//Delete the dist folder when clean:dist is run
gulp.task('clean:dist', function () {
    return del.sync('dist');
});

//Running a sequence of tasks. Tasks in an array will run in parallel, comma seperated ones will run in order
gulp.task('cleanBuild', function (callback) {
    sequence('clean:dist', //Clean the dist folder first (deletes everything)
        ['sassToCss', 'combo'], //Convert the sass and combine and minify files, putting the output in the dist folder
        callback);
});
