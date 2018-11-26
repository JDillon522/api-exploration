'use strict';

const gulp = require('gulp');
const sass = require('gulp-sass');
const autoprefixer = require('gulp-autoprefixer');
const tildeImporter = require('node-sass-tilde-importer');

sass.compiler = require('node-sass');
const sassOptions = {
    importer: tildeImporter
}

const paths = {
    scssRoot: './Views/',
    outputRoot: './wwwroot/'
};

paths.stylesRoot = paths.scssRoot + 'site.scss';
paths.styles = paths.scssRoot + '**/*.scss';
paths.stylesDest = paths.outputRoot + 'css';

gulp.task('scss', () => {
    return gulp.src(paths.stylesRoot)
        .pipe(sass(sassOptions).on('error', sass.logError))
        .pipe(gulp.dest(paths.stylesDest));
});

gulp.task('autoprefixer', () =>{
    return gulp.src(paths.stylesDest)
        .pipe(autoprefixer({}))
        .pipe(gulp.dest('dist'))
    }
);

gulp.task('scss:watch', () => {
    console.log(`Watching files at: ${paths.styles}`);

    return gulp.watch(paths.styles, gulp.series(['scss', 'autoprefixer']));
});

// A 'default' task is required by Gulp v4
gulp.task('default', gulp.series(['scss', 'autoprefixer', 'scss:watch']));