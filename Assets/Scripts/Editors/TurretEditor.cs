using UnityEngine;
using UnityEditor;
using Turret;

[CustomEditor(typeof(TurretBase))]
public class TurretEditor : Editor
{
    [HideInInspector, SerializeField] private TurretData _turretData;
    bool showMachineGun = true;
    bool showLaserGun = true;
    bool showMissileGun = true;
    bool showDamageMulti = true;


    private void OnEnable()
    {
        _turretData = serializedObject.FindProperty("_turretData").objectReferenceValue as TurretData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.BoldAndItalic,
            fontSize = 20
        };
        labelStyle.normal.textColor = Color.white;

        GUILayout.Label("Turret properties:", labelStyle);

        _turretData.MaxHealth = EditorGUILayout.FloatField("Max health", _turretData.MaxHealth);
        _turretData.AimSpeed = EditorGUILayout.Slider("Aim speed", _turretData.AimSpeed, 50.0f, 360.0f);

        GUILayout.Label("Tweeking Turret when receive Damage");
        _turretData.ShakeTime = EditorGUILayout.Slider("Shake time", _turretData.ShakeTime, 0.2f, 1.0f);
        _turretData.ShakeForce = EditorGUILayout.Slider("Shake force", _turretData.ShakeForce, 0.05f, 1.0f);

        _turretData.RetractitleCannonSpeed = EditorGUILayout.Slider("Rectractile cannon speed", _turretData.RetractitleCannonSpeed, 1.0f, 10.0f);
        _turretData.SmokeMaxEmission = EditorGUILayout.Slider("Shake force", _turretData.SmokeMaxEmission, 1.0f, 10.0f);

        GUILayout.Space(25.0f);
        GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.BoldAndItalic,
            fontSize = 14
        };
        Color myStyleColor = Color.white;
        myFoldoutStyle.normal.textColor = myStyleColor;

        showDamageMulti = EditorGUILayout.Foldout(showDamageMulti, "Damage Multiplier", myFoldoutStyle);
        if (showDamageMulti)
        {
            _turretData.DamageMulti[0] = EditorGUILayout.FloatField("Player 1", _turretData.DamageMulti[0]);
            _turretData.DamageMulti[1] = EditorGUILayout.FloatField("Player 2", _turretData.DamageMulti[1]);
            _turretData.DamageMulti[2] = EditorGUILayout.FloatField("Player 3", _turretData.DamageMulti[2]);
            _turretData.DamageMulti[3] = EditorGUILayout.FloatField("Player 4", _turretData.DamageMulti[3]);
        }


        showMachineGun = EditorGUILayout.Foldout(showMachineGun, "Machine gun", myFoldoutStyle);
        if (showMachineGun)
        {
            _turretData.machineGun.damage = EditorGUILayout.FloatField("Damage", _turretData.machineGun.damage);
            _turretData.machineGun.moveSpeed = EditorGUILayout.FloatField("Bullet speed", _turretData.machineGun.moveSpeed);
            _turretData.machineGun.fireRate = EditorGUILayout.FloatField("Fire rate", _turretData.machineGun.fireRate);
            _turretData.machineGun.maxAmmo = EditorGUILayout.FloatField("Max ammo", _turretData.machineGun.maxAmmo);
            _turretData.machineGun.spreadBullet = EditorGUILayout.FloatField("Spread bullet factor", _turretData.machineGun.spreadBullet);
            _turretData.machineGun.recoilForce = EditorGUILayout.FloatField("Recoil Force", _turretData.machineGun.recoilForce);
        }

        showLaserGun = EditorGUILayout.Foldout(showLaserGun, "Laser gun", myFoldoutStyle);
        if (showLaserGun)
        {
            _turretData.laserGun.damage = EditorGUILayout.FloatField("Damage", _turretData.laserGun.damage);
            _turretData.laserGun.range = EditorGUILayout.FloatField("Range", _turretData.laserGun.range);
            _turretData.laserGun.ammoConsumeRate = EditorGUILayout.FloatField("Ammo consume rate", _turretData.laserGun.ammoConsumeRate);
            _turretData.laserGun.aimSpeedMultiplier = EditorGUILayout.FloatField("Aim speed", _turretData.laserGun.aimSpeedMultiplier);
            _turretData.laserGun.maxAmmo = EditorGUILayout.FloatField("Max ammo", _turretData.laserGun.maxAmmo);
            _turretData.laserGun.recoildForce = EditorGUILayout.FloatField("Recoil Force", _turretData.laserGun.recoildForce);
        }

        showMissileGun = EditorGUILayout.Foldout(showMissileGun, "Missile gun", myFoldoutStyle);
        if (showMissileGun)
        {
            _turretData.missileGun.damage = EditorGUILayout.FloatField("Damage", _turretData.missileGun.damage);
            EditorGUILayout.LabelField("Min speed:", $"{_turretData.missileGun.minSpeed}");
            EditorGUILayout.LabelField("Max speed:", $"{_turretData.missileGun.maxSpeed}");
            EditorGUILayout.MinMaxSlider(ref _turretData.missileGun.minSpeed, ref _turretData.missileGun.maxSpeed, 0.05f, 10.0f);
            GUILayout.Space(25.0f);
            _turretData.missileGun.acceleration = EditorGUILayout.FloatField("Acceleration", _turretData.missileGun.acceleration);
            _turretData.missileGun.fireRate = EditorGUILayout.FloatField("Fire rate", _turretData.missileGun.fireRate);
            _turretData.missileGun.radiusEffect = EditorGUILayout.FloatField("Radius effect", _turretData.missileGun.radiusEffect);
            _turretData.missileGun.maxAmmo = EditorGUILayout.FloatField("Max ammo", _turretData.missileGun.maxAmmo);
            _turretData.missileGun.recoilForce = EditorGUILayout.FloatField("Recoil force", _turretData.missileGun.recoilForce);
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(_turretData);
    }

}
