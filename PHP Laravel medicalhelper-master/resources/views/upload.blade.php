@extends('layouts.app')

@section('content')
<div class="container">
    <div class="row justify-content-center">
        <div class="col-md-8">

            <form class="" action="{{ route('upload.store') }}" method="post" enctype="multipart/form-data">
                @csrf

                <div class="d-flex justify-content-center align-items-end gap-3">
                    <div class="">
                        <label for="formFileMultiple" class="form-label">Загрузка данных для анализа</label>
                        <input class="form-control" type="file" name="file" id="formFileMultiple" required>
                    </div>
                    <div class="">
                        <button class="btn btn-primary" type="submit">Загрузить</button>
                    </div>
                </div>
            </form>
            <br>
            @if ($errors->any())
            <div class="alert alert-danger">
                <ul>
                    @foreach ($errors->all() as $error)
                    <li>{{ $error }}</li>
                    @endforeach
                </ul>
            </div>
            @endif()

            @isset($response)
            @foreach($response as $responseData)
            <div class="alert alert-success">
                <ul>
                    <li>
                        <div>{{ $responseData['messageToDisplay'] }}</div>
                        <div>Файл: {{ $responseData['fileName'] }}</div>
                        <div>Строк прочитано: {{ $responseData['rowsReaded'] }}</div>
                        <div>Строк загружено: {{ $responseData['rowsUploaded'] }}</div>
                        @if($responseData['rowsWithBrokenBirthdayDate'])
                        <div class="text-danger">
                            Номера строк с некорректной датой рождения:
                            @foreach ($responseData['rowsWithBrokenBirthdayDate'] as $value)
                            {{ $loop->first ? '' : ', ' }}
                            <span class="nice">{{ $value }}</span>
                            @endforeach
                        </div>
                        @endif()
                        @if($responseData['rowsWithBrokenTreatDate'])
                        <div class="text-danger">Номера строк с некорректной датой приема:
                            @foreach ($responseData['rowsWithBrokenTreatDate'] as $value)
                            {{ $loop->first ? '' : ', ' }}
                            <span class="nice">{{ $value }}</span>
                            @endforeach
                        </div>
                        @endif()

                    </li>
                </ul>
            </div>
            @endforeach
            @endisset
        </div>
    </div>
</div>
@endsection