<?php

use Illuminate\Support\Facades\Route;
//use App\Http\Controllers\UploadController;
//use App\Http\Controllers\HomeController;
/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider and all of them will
| be assigned to the "web" middleware group. Make something great!
|
*/


Auth::routes();

Route::get('/', [App\Http\Controllers\HomeController::class, 'index'])->name('home');

Route::post('/upload', [App\Http\Controllers\UploadController::class, 'store'])->name('upload.store');
Route::get('/upload', [App\Http\Controllers\UploadController::class, 'index'])->name('upload.index');

Route::get('/analysis', [App\Http\Controllers\AnalysisController::class, 'index'])->name('analysis.index');
Route::post('/analysis', [App\Http\Controllers\AnalysisController::class, 'store'])->name('analysis.store');

Route::get('/comparison', [App\Http\Controllers\ComparisonController::class, 'index'])->name('comparison.index');
Route::get('/batchAssignments', [App\Http\Controllers\BatchAssignmentsController::class, 'index'])->name('batchAssignments.index');
Route::get('/generalStandards', [App\Http\Controllers\GeneralStandardsController::class, 'index'])->name('generalStandards.index');

Route::get('/dashboard', [App\Http\Controllers\DashboardController::class, 'index'])->name('dashboard.index');
Route::post('/dashboard', [App\Http\Controllers\DashboardController::class, 'store'])->name('dashboard.store');
