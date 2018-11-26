'use strict';

const gulp = require('gulp');
const sass = require('gulp-sass');
sass.compiler = require('node-sass');
const rimraf = require('rimraf');

const paths = {
    webroot: './wwwroot/'
};

paths.stylesRoot = paths.webroot + 'scss/site.scss';
paths.styles = paths.webroot + 'scss/**/*.scss';
paths.stylesDest = paths.webroot + 'css';

gulp.task('scss', () => {
    console.log('Updating Scss...');

    return gulp.src(paths.stylesRoot)
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest(paths.stylesDest));
});

gulp.task('scss:watch', () => {
    console.log(`Watching files at: ${paths.styles}`);

    return gulp.watch(paths.styles, gulp.series(['scss']));
});

// A 'default' task is required by Gulp v4
gulp.task('default', gulp.series(['scss', 'scss:watch']));