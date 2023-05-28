<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;


class UploadController extends Controller
{
    public function __construct()
    {
        $this->middleware('auth');
    }

    public function index()
    {

        return view('upload');

    }

    public function store(Request $request)
    {

        $validated = $request->validate([
            'file' => 'required|file|mimes:xls,xlsx|max:28672',
        ]);

        $file = $validated['file'];

        $response = Http::connectTimeout(180)->withHeaders(['XApiKey' => env('API_KEY')])->attach(
            'attachment', file_get_contents($file), $file->getClientOriginalName()
        )->post(env('API_URL').'/UploadFile');


        if ($response->ok()) {
            $response = $response->json();
            $data = compact('response');
            return view('upload', $data);
        } else {
            return back()->withErrors(['msg' => $response->body()]);
        }


        //todo $errors добавить в них ответ сервера
        //todo перевести ошибки на русский

    }


}
