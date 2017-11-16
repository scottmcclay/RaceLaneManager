/// <binding AfterBuild='default' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var ts = require('gulp-typescript');
var targetDir = "../bin/Debug/website";
var tsProject = ts.createProject('tsconfig.json', { "rootDir": ".", "outDir": targetDir });

gulp.task('ts', function () {
    tsProject.src()
        .pipe(tsProject())
        .js.pipe(gulp.dest(targetDir));
});

gulp.task('ts:watch', function () {
    
})

gulp.task('dist', function () {
    gulp.src([
        'bower_components/**/*',
        'images/**/*',
        'Scripts/**/*',
        'web_components/**/*.html',
        'web_components/**/*.css',
        '*.html'
    ], { base: '.' })
        .pipe(gulp.dest(targetDir));
});

gulp.task('default', ['ts', 'dist']);
