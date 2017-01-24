/* global require, __dirname */
var gulp = require('gulp');
var sourcemaps = require('gulp-sourcemaps');
var babel = require('gulp-babel');
var babelExternalHelpers = require('gulp-babel-external-helpers');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var less = require('gulp-less');
var jshint = require('gulp-jshint');
var watch = require('gulp-watch');
var filter = require('gulp-filter');
var order = require('gulp-order');
var nodemon = require('gulp-nodemon');
var livereload = require('gulp-livereload');
var filesize = require('gulp-size');
var gulpif = require('gulp-if');
var imagemin = require('gulp-imagemin');
var pngquant = require('imagemin-pngquant');
var rev = require('gulp-rev');
var gzip = require('gulp-gzip');
var requirejs = require('requirejs');
var beep = require('beepbeep');
var argv = require('yargs').argv;
var merge = require('merge-stream');
var clone = require('gulp-clone');

var loadedBrotli = false;
var brotli;
try {
  brotli = require('gulp-brotli');
  loadedBrotli = true;
} catch(e) {
  loadedBrotli = false;
}

// Uglify-options https://github.com/mishoo/UglifyJS2#the-simple-way

gulp.task('default', ['build']);
gulp.task('build', ['jshint', 'js', 'vendor-js', 'less', 'fonts', 'images', 'svg']);

gulp.task('watch-base', ['build'], function () {
  livereload.listen();

  //watches .js and .less files
  //we used to do batch(function()...) in the callback but it seems to mess with running the tasks
  watch('Scripts/**/*.js', { verbose: true }, function () {
    gulp.start('js');
  });

  watch('Assets/styles/**/*.less', { verbose: true }, function () {
    gulp.start('less');
  });

  watch('Assets/images/**/*.{jpg,png,gif}', { verbose: true }, function () {
    gulp.start('images');
  });

  watch('Assets/fonts/**/*.*', { verbose: true }, function () {
    gulp.start('fonts');
  });
});


gulp.task('watch-dotnet', function() {
    //also monitors .cs and .json file (asp.net stuff) and restarts the k runtime on file changes
    //var exec = 'dnx kestrel --server.urls http://*:5000';
    var exec = 'dotnet run';

    nodemon({
        ext: 'cs',
        ignore: [ './obj/**', './bin/**' ],
        verbose: false,
        exec: exec,
        watch: [
          './',
        ]
    }).on('restart', function (changedFiles) {
        console.log('Restarting!');
        if(!!changedFiles && changedFiles.length > 0) {
          changedFiles.forEach((item) => {
            console.log(item);
          });
        }
    });
});

gulp.task('watch', ['watch-base', 'watch-dotnet'], function () { });

gulp.task('images', function () {
  return gulp.src('Assets/images/**/*.*')
    .pipe(imagemin({
      progressive: true,
      use: [pngquant()]
    }))
    .pipe(gulp.dest('wwwroot/images'));
});

gulp.task('fonts', function () {
  return gulp.src('Assets/fonts/**/*.*')
    .pipe(gulp.dest('wwwroot/fonts/'))
    .pipe(livereload());
});

gulp.task('svg', function () {
  return gulp.src('Assets/svg/**/*.svg')
    .pipe(gulp.dest('wwwroot/svg/'));
});

gulp.task('less', function () {
  var base = gulp.src('Assets/styles/main.less')
    .pipe(sourcemaps.init())
    .pipe(less({ compress: true }))
    .on('error', function(e) {
        console.error('>>> ERROR IN LESS', e);
        beep(2, 500);
        this.emit('end');
      })
    .pipe(sourcemaps.write('.'))
    .pipe(gulp.dest('wwwroot/css'))
    .pipe(rev())
    .pipe(gulp.dest('wwwroot/css'));

  return preEncodeResources(base, 'wwwroot/css').pipe(livereload());
});

gulp.task('js', ['jshint'], function () {
  var base = gulp.src('Assets/scripts/*.js')
    .pipe(jshint())
    .pipe(sourcemaps.init())
    .pipe(babel({
      modules: 'amd',
      moduleIds: true,
      moduleRoot: 'web',
      externalHelpers: true
    })).on('error', function(e) {
      console.error('>>> ERROR', e);
      beep(2, 500);
      this.emit('end');
    })
    .pipe(babelExternalHelpers('babelHelpers.js'))
    .pipe(concat('scripts.js'))
    .pipe(gulpif(argv.environment === 'production', uglify({
      mangle: true,
      compress: true
    })))
    //.pipe(sourcemaps.write('.'))
    .pipe(gulp.dest('wwwroot/js'))
    .pipe(rev())
    .pipe(gulp.dest('wwwroot/js'));

  return preEncodeResources(base, 'wwwroot/js').pipe(livereload());
});

gulp.task('jshint', function () {
  return gulp.src('Assets/scripts/**/*.js')
    .pipe(jshint())
    .pipe(jshint.reporter('jshint-stylish'))
    .pipe(gulpif(argv.environment === 'production', jshint.reporter('fail')));
});

gulp.task('vendor-js', ['vendor-requirejs'], function () {
  var base = gulp.src('./bower_components/**/*.js')
   .pipe(filter([
     'almond/almond.js',
     'all.js'
   ]))
    .pipe(order([
      'almond/almond.js',    // Start with almond
      '**/*.js'              // Eeeeeeverything else
    ]))
    .pipe(filesize({ showFiles: true }))
    //.pipe(sourcemaps.init())
    .pipe(concat('vendor.js'))
    .pipe(gulpif(argv.environment === 'production', uglify({
      mangle: true,
      compress: true
    })))
    .pipe(filesize({ showFiles: true }))
    //.pipe(sourcemaps.write('.'))
    .pipe(gulp.dest('wwwroot/js'))
    .pipe(rev())
    .pipe(gulp.dest('wwwroot/js'));

  return preEncodeResources(base, 'wwwroot/js').pipe(livereload());
});


gulp.task('vendor-requirejs', function (done) {
  requirejs.optimize({
    baseUrl: './',
    generateSourceMaps: true,
    //logLevel: 0,
    include: [
      'jquery',
      'promise',
      'moment',
      'moment-sv'
    ],
    optimize: 'none',
    out: './bower_components/all.js',
    paths: {
      'jquery': 'bower_components/jquery/jquery',
      'promise': 'bower_components/es6-promise-polyfill/promise',
      'moment': 'bower_components/moment/moment',
      'moment-sv': 'bower_components/moment/locale/sv'
    },
    'preserveLicenseComments': true,
    'wrapShim': false
  }, function () {
    done();
  }, done);
});

var preEncodeResources = function(baseStream, outPath) {
  var streams = [];
  if(argv.environment === 'production') {
    streams.push(baseStream
      .pipe(clone())
      .pipe(gzip({ append: true }))
      .pipe(gulp.dest(outPath)));

    var MODE_GENERIC = 0,
        MODE_TEXT = 1;

    streams.push(baseStream
      .pipe(clone())
      .pipe(gulpif(loadedBrotli, brotli.compress({
        extension: 'br',
        skipLarger: true,
        mode: MODE_TEXT,
        quality: 11
      })))
      .pipe(gulp.dest(outPath)));
  }

  streams.push(baseStream.pipe(rev.manifest('wwwroot/rev-manifest.json', {
      base: 'wwwroot/',
      merge: true
    }))
    .pipe(gulp.dest('wwwroot/')));

  return merge.apply(null, streams);
}
