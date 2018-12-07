'use strict';

const gulp = require('gulp');
const sass = require('gulp-sass');
const autoprefixer = require('gulp-autoprefixer');
const tildeImporter = require('node-sass-tilde-importer');
const concat = require('gulp-concat');

sass.compiler = require('node-sass');
const sassOptions = {
    importer: tildeImporter
}

const root = {
    app: './Views/',
    output: './wwwroot/'
};

const paths = {
    sass: [root.app + '**/*.scss'],
    js: [root.output + 'js/utils.js', root.app + '**/*.js']
};

root.stylesRoot = root.app + 'site.scss';
root.stylesDest = root.output + 'css';

root.js = root.app + '**/*.js';
root.jsDest = root.output + 'js';

gulp.task('scss', () => {
    return gulp.src(paths.sass)
        .pipe(sass(sassOptions).on('error', sass.logError))
        .pipe(concat('site.css'))
        .pipe(gulp.dest(root.stylesDest));
});

// gulp.task('autoprefixer', () =>{
//     return gulp.src(root.stylesDest)
//         .pipe(autoprefixer({}))
//         .pipe(gulp.dest(root.output))
// });

gulp.task('js', () => {
    return gulp.src(paths.js)
        .pipe(concat('site.js'))
        .pipe(gulp.dest(root.jsDest));
});

gulp.task('watch', () => {
    gulp.watch(paths.js, gulp.series(['js']));
    gulp.watch(paths.sass, gulp.series(['scss', /* 'autoprefixer' */]));
});

// A 'default' task is required by Gulp v4
gulp.task('default', gulp.series(['scss', /* 'autoprefixer', */ 'js', 'watch']));