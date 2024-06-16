using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class HoboCharacterController : MonoBehaviour
{
    [SerializeField]
    public Animator animator;

    [SerializeField]
    private Rigidbody rigidbody;
    private CharacterController controller;
    private GameObject mainCamera;
    private StarterAssetsInputs input;

    [SerializeField]

    // player
    private float animationBlend;
    private float jumpForce = 7f;
    private bool isGrounded = false;
    private float speed;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    private Dictionary<string, int> actions = new Dictionary<string, int>();

    [HideInInspector]
    public int actionID = 0;

    [SerializeField]
    private bool isJumping = false;

    [SerializeField]
    private bool EnableDoubleJumping = true;

    [SerializeField]
    private bool isDoubleJumping = false;

    [SerializeField]
    private bool isFalling = false;

    private bool animationHasLoop = false;
    private bool actionNoLoopedReturnToIdle = true;
    private AnimatorClipInfo[] animatorInfo;
    private AnimationClip currentAnimationClip;
    private string currentAnimation;


    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;


    [Header("Actions Names")]
    private string A_001_POSE_1 = "001_pose_1";

    private string A_002_POSE_2 = "002_pose_2";
    private string A_003_POSE_3 = "003_pose_3";
    private string A_004_POSE_4 = "004_pose_4";
    private string A_005_POSE_5 = "005_pose_5";
    private string A_006_POSE_6 = "006_pose_6";
    private string A_007_POSE_7 = "007_pose_7";
    private string A_008_POSE_8 = "008_pose_8";
    private string A_009_POSE_9 = "009_pose_9";
    private string A_010_POSE_10 = "010_pose_10";
    private string A_011_IDLE_1 = "011_idle_1";
    private string A_012_IDLE_2 = "012_idle_2";
    private string A_013_IDLE_3 = "013_idle_3";
    private string A_014_IDLE_4 = "014_idle_4";
    private string A_015_IDLE_5 = "015_idle_5";
    private string A_016_IDLE_6 = "016_idle_6";
    private string A_017_IDLE_7 = "017_idle_7";
    private string A_018_IDLE_8 = "018_idle_8";
    private string A_019_IDLE_9 = "019_idle_9";
    private string A_020_IDLE_10 = "020_idle_10";
    private string A_021_WALK_1 = "021_walk_1";
    private string A_022_WALK_2 = "022_walk_2";
    private string A_023_WALK_3 = "023_walk_3";
    private string A_024_WALK_4 = "024_walk_4";
    private string A_025_WALK_5 = "025_walk_5";
    private string A_026_WALK_6 = "026_walk_6";
    private string A_027_WALK_7 = "027_walk_7";
    private string A_028_WALK_8 = "028_walk_8";
    private string A_029_WALK_9 = "029_walk_9";
    private string A_030_WALK_10 = "030_walk_10";
    private string A_031_RUN_1 = "031_run_1";
    private string A_032_RUN_2 = "032_run_2";
    private string A_033_RUN_3 = "033_run_3";
    private string A_034_RUN_4 = "034_run_4";
    private string A_035_RUN_5 = "035_run_5";
    private string A_036_RUN_6 = "036_run_6";
    private string A_037_RUN_7 = "037_run_7";
    private string A_038_RUN_8 = "038_run_8";
    private string A_039_RUN_9 = "039_run_9";
    private string A_040_RUN_10 = "040_run_10";
    private string A_041_JUMP_1 = "041_jump_1";
    private string A_042_JUMP_2 = "042_jump_2";
    private string A_043_JUMP_3 = "043_jump_3";
    private string A_044_JUMP_4 = "044_jump_4";
    private string A_045_JUMP_5 = "045_jump_5";
    private string A_046_JUMP_6 = "046_jump_6";
    private string A_047_JUMP_7 = "047_jump_7";
    private string A_048_JUMP_8 = "048_jump_8";
    private string A_049_JUMP_9 = "049_jump_9";
    private string A_050_JUMP_10 = "050_jump_10";
    private string A_051_JUMP_SPIN_1 = "051_jump_spin_1";
    private string A_052_JUMP_SPIN_2 = "052_jump_spin_2";
    private string A_053_JUMP_SPIN_3 = "053_jump_spin_3";
    private string A_054_JUMP_SPIN_4 = "054_jump_spin_4";
    private string A_055_JUMP_SPIN_5 = "055_jump_spin_5";
    private string A_056_JUMP_SPIN_6 = "056_jump_spin_6";
    private string A_057_JUMP_SPIN_7 = "057_jump_spin_7";
    private string A_058_JUMP_SPIN_8 = "058_jump_spin_8";
    private string A_059_JUMP_SPIN_9 = "059_jump_spin_9";
    private string A_060_JUMP_SPIN_10 = "060_jump_spin_10";
    private string A_061_FALL_1 = "061_fall_1";
    private string A_062_FALL_2 = "062_fall_2";
    private string A_063_FALL_3 = "063_fall_3";
    private string A_064_FALL_4 = "064_fall_4";
    private string A_065_FALL_5 = "065_fall_5";
    private string A_066_FALL_6 = "066_fall_6";
    private string A_067_FALL_7 = "067_fall_7";
    private string A_068_FALL_8 = "068_fall_8";
    private string A_069_FALL_9 = "069_fall_9";
    private string A_070_FALL_10 = "070_fall_10";
    private string A_071_LAND_1 = "071_land_1";
    private string A_072_LAND_2 = "072_land_2";
    private string A_073_LAND_3 = "073_land_3";
    private string A_074_LAND_4 = "074_land_4";
    private string A_075_LAND_5 = "075_land_5";
    private string A_076_LAND_6 = "076_land_6";
    private string A_077_LAND_7 = "077_land_7";
    private string A_078_LAND_8 = "078_land_8";
    private string A_079_LAND_9 = "079_land_9";
    private string A_080_LAND_10 = "080_land_10";
    private string A_081_PUNCH_1 = "081_punch_1";
    private string A_082_PUNCH_2 = "082_punch_2";
    private string A_083_PUNCH_3 = "083_punch_3";
    private string A_084_PUNCH_4 = "084_punch_4";
    private string A_085_PUNCH_5 = "085_punch_5";
    private string A_086_PUNCH_6 = "086_punch_6";
    private string A_087_PUNCH_7 = "087_punch_7";
    private string A_088_PUNCH_8 = "088_punch_8";
    private string A_089_PUNCH_9 = "089_punch_9";
    private string A_090_PUNCH_10 = "090_punch_10";
    private string A_091_KICK_1 = "091_kick_1";
    private string A_092_KICK_2 = "092_kick_2";
    private string A_093_KICK_3 = "093_kick_3";
    private string A_094_KICK_4 = "094_kick_4";
    private string A_095_KICK_5 = "095_kick_5";
    private string A_096_KICK_6 = "096_kick_6";
    private string A_097_KICK_7 = "097_kick_7";
    private string A_098_KICK_8 = "098_kick_8";
    private string A_099_KICK_9 = "099_kick_9";
    private string A_100_KICK_10 = "100_kick_10";
    private string A_101_ATTACK_1 = "101_attack_1";
    private string A_102_ATTACK_2 = "102_attack_2";
    private string A_103_ATTACK_3 = "103_attack_3";
    private string A_104_ATTACK_4 = "104_attack_4";
    private string A_105_ATTACK_5 = "105_attack_5";
    private string A_106_ATTACK_6 = "106_attack_6";
    private string A_107_ATTACK_7 = "107_attack_7";
    private string A_108_ATTACK_8 = "108_attack_8";
    private string A_109_ATTACK_9 = "109_attack_9";
    private string A_110_ATTACK_10 = "110_attack_10";
    private string A_111_MAGIC_1 = "111_magic_1";
    private string A_112_MAGIC_2 = "112_magic_2";
    private string A_113_MAGIC_3 = "113_magic_3";
    private string A_114_MAGIC_4 = "114_magic_4";
    private string A_115_MAGIC_5 = "115_magic_5";
    private string A_116_MAGIC_6 = "116_magic_6";
    private string A_117_MAGIC_7 = "117_magic_7";
    private string A_118_MAGIC_8 = "118_magic_8";
    private string A_119_MAGIC_9 = "119_magic_9";
    private string A_120_MAGIC_10 = "120_magic_10";
    private string A_121_BLOCK_1 = "121_block_1";
    private string A_122_BLOCK_2 = "122_block_2";
    private string A_123_BLOCK_3 = "123_block_3";
    private string A_124_BLOCK_4 = "124_block_4";
    private string A_125_BLOCK_5 = "125_block_5";
    private string A_126_BLOCK_6 = "126_block_6";
    private string A_127_BLOCK_7 = "127_block_7";
    private string A_128_BLOCK_8 = "128_block_8";
    private string A_129_BLOCK_9 = "129_block_9";
    private string A_130_BLOCK_10 = "130_block_10";
    private string A_131_HIT_1 = "131_hit_1";
    private string A_132_HIT_2 = "132_hit_2";
    private string A_133_HIT_3 = "133_hit_3";
    private string A_134_HIT_4 = "134_hit_4";
    private string A_135_HIT_5 = "135_hit_5";
    private string A_136_HIT_6 = "136_hit_6";
    private string A_137_HIT_7 = "137_hit_7";
    private string A_138_HIT_8 = "138_hit_8";
    private string A_139_HIT_9 = "139_hit_9";
    private string A_140_HIT_10 = "140_hit_10";
    private string A_141_LOSE_1 = "141_lose_1";
    private string A_142_LOSE_2 = "142_lose_2";
    private string A_143_LOSE_3 = "143_lose_3";
    private string A_144_LOSE_4 = "144_lose_4";
    private string A_145_LOSE_5 = "145_lose_5";
    private string A_146_LOSE_6 = "146_lose_6";
    private string A_147_LOSE_7 = "147_lose_7";
    private string A_148_LOSE_8 = "148_lose_8";
    private string A_149_LOSE_9 = "149_lose_9";
    private string A_150_LOSE_10 = "150_lose_10";
    private string A_151_DIE_1 = "151_die_1";
    private string A_152_DIE_2 = "152_die_2";
    private string A_153_DIE_3 = "153_die_3";
    private string A_154_DIE_4 = "154_die_4";
    private string A_155_DIE_5 = "155_die_5";
    private string A_156_DIE_6 = "156_die_6";
    private string A_157_DIE_7 = "157_die_7";
    private string A_158_DIE_8 = "158_die_8";
    private string A_159_DIE_9 = "159_die_9";
    private string A_160_DIE_10 = "160_die_10";
    private string A_161_VICTORY_1 = "161_victory_1";
    private string A_162_VICTORY_2 = "162_victory_2";
    private string A_163_VICTORY_3 = "163_victory_3";
    private string A_164_VICTORY_4 = "164_victory_4";
    private string A_165_VICTORY_5 = "165_victory_5";
    private string A_166_VICTORY_6 = "166_victory_6";
    private string A_167_VICTORY_7 = "167_victory_7";
    private string A_168_VICTORY_8 = "168_victory_8";
    private string A_169_VICTORY_9 = "169_victory_9";
    private string A_170_VICTORY_10 = "170_victory_10";
    private string A_171_EXTRA_ACTIONS_1 = "171_extra_actions_1";
    private string A_172_EXTRA_ACTIONS_2 = "172_extra_actions_2";
    private string A_173_EXTRA_ACTIONS_3 = "173_extra_actions_3";
    private string A_174_EXTRA_ACTIONS_4 = "174_extra_actions_4";
    private string A_175_EXTRA_ACTIONS_5 = "175_extra_actions_5";
    private string A_176_EXTRA_ACTIONS_6 = "176_extra_actions_6";
    private string A_177_EXTRA_ACTIONS_7 = "177_extra_actions_7";
    private string A_178_EXTRA_ACTIONS_8 = "178_extra_actions_8";
    private string A_179_EXTRA_ACTIONS_9 = "179_extra_actions_9";
    private string A_180_EXTRA_ACTIONS_10 = "180_extra_actions_10";
    private string A_181_CUSTOM_POSE_1 = "181_custom_pose_1";
    private string A_182_CUSTOM_POSE_2 = "182_custom_pose_2";
    private string A_183_CUSTOM_POSE_3 = "183_custom_pose_3";
    private string A_184_CUSTOM_POSE_4 = "184_custom_pose_4";
    private string A_185_CUSTOM_POSE_5 = "185_custom_pose_5";
    private string A_186_CUSTOM_POSE_6 = "186_custom_pose_6";
    private string A_187_CUSTOM_POSE_7 = "187_custom_pose_7";
    private string A_188_CUSTOM_POSE_8 = "188_custom_pose_8";
    private string A_189_CUSTOM_POSE_9 = "189_custom_pose_9";
    private string A_190_CUSTOM_POSE_10 = "190_custom_pose_10";
    private string A_191_CUSTOM_IDLE_1 = "191_custom_idle_1";
    private string A_192_CUSTOM_IDLE_2 = "192_custom_idle_2";
    private string A_193_CUSTOM_IDLE_3 = "193_custom_idle_3";
    private string A_194_CUSTOM_IDLE_4 = "194_custom_idle_4";
    private string A_195_CUSTOM_IDLE_5 = "195_custom_idle_5";
    private string A_196_CUSTOM_IDLE_6 = "196_custom_idle_6";
    private string A_197_CUSTOM_IDLE_7 = "197_custom_idle_7";
    private string A_198_CUSTOM_IDLE_8 = "198_custom_idle_8";
    private string A_199_CUSTOM_IDLE_9 = "199_custom_idle_9";
    private string A_200_CUSTOM_IDLE_10 = "200_custom_idle_10";
    private string A_201_CUSTOM_WALK_1 = "201_custom_walk_1";
    private string A_202_CUSTOM_WALK_2 = "202_custom_walk_2";
    private string A_203_CUSTOM_WALK_3 = "203_custom_walk_3";
    private string A_204_CUSTOM_WALK_4 = "204_custom_walk_4";
    private string A_205_CUSTOM_WALK_5 = "205_custom_walk_5";
    private string A_206_CUSTOM_WALK_6 = "206_custom_walk_6";
    private string A_207_CUSTOM_WALK_7 = "207_custom_walk_7";
    private string A_208_CUSTOM_WALK_8 = "208_custom_walk_8";
    private string A_209_CUSTOM_WALK_9 = "209_custom_walk_9";
    private string A_210_CUSTOM_WALK_10 = "210_custom_walk_10";
    private string A_211_CUSTOM_RUN_1 = "211_custom_run_1";
    private string A_212_CUSTOM_RUN_2 = "212_custom_run_2";
    private string A_213_CUSTOM_RUN_3 = "213_custom_run_3";
    private string A_214_CUSTOM_RUN_4 = "214_custom_run_4";
    private string A_215_CUSTOM_RUN_5 = "215_custom_run_5";
    private string A_216_CUSTOM_RUN_6 = "216_custom_run_6";
    private string A_217_CUSTOM_RUN_7 = "217_custom_run_7";
    private string A_218_CUSTOM_RUN_8 = "218_custom_run_8";
    private string A_219_CUSTOM_RUN_9 = "219_custom_run_9";
    private string A_220_CUSTOM_RUN_10 = "220_custom_run_10";
    private string A_221_CUSTOM_JUMP_1 = "221_custom_jump_1";
    private string A_222_CUSTOM_JUMP_2 = "222_custom_jump_2";
    private string A_223_CUSTOM_JUMP_3 = "223_custom_jump_3";
    private string A_224_CUSTOM_JUMP_4 = "224_custom_jump_4";
    private string A_225_CUSTOM_JUMP_5 = "225_custom_jump_5";
    private string A_226_CUSTOM_JUMP_6 = "226_custom_jump_6";
    private string A_227_CUSTOM_JUMP_7 = "227_custom_jump_7";
    private string A_228_CUSTOM_JUMP_8 = "228_custom_jump_8";
    private string A_229_CUSTOM_JUMP_9 = "229_custom_jump_9";
    private string A_230_CUSTOM_JUMP_10 = "230_custom_jump_10";
    private string A_231_CUSTOM_JUMP_SPIN_1 = "231_custom_jump_spin_1";
    private string A_232_CUSTOM_JUMP_SPIN_2 = "232_custom_jump_spin_2";
    private string A_233_CUSTOM_JUMP_SPIN_3 = "233_custom_jump_spin_3";
    private string A_234_CUSTOM_JUMP_SPIN_4 = "234_custom_jump_spin_4";
    private string A_235_CUSTOM_JUMP_SPIN_5 = "235_custom_jump_spin_5";
    private string A_236_CUSTOM_JUMP_SPIN_6 = "236_custom_jump_spin_6";
    private string A_237_CUSTOM_JUMP_SPIN_7 = "237_custom_jump_spin_7";
    private string A_238_CUSTOM_JUMP_SPIN_8 = "238_custom_jump_spin_8";
    private string A_239_CUSTOM_JUMP_SPIN_9 = "239_custom_jump_spin_9";
    private string A_240_CUSTOM_JUMP_SPIN_10 = "240_custom_jump_spin_10";
    private string A_241_CUSTOM_FALL_1 = "241_custom_fall_1";
    private string A_242_CUSTOM_FALL_2 = "242_custom_fall_2";
    private string A_243_CUSTOM_FALL_3 = "243_custom_fall_3";
    private string A_244_CUSTOM_FALL_4 = "244_custom_fall_4";
    private string A_245_CUSTOM_FALL_5 = "245_custom_fall_5";
    private string A_246_CUSTOM_FALL_6 = "246_custom_fall_6";
    private string A_247_CUSTOM_FALL_7 = "247_custom_fall_7";
    private string A_248_CUSTOM_FALL_8 = "248_custom_fall_8";
    private string A_249_CUSTOM_FALL_9 = "249_custom_fall_9";
    private string A_250_CUSTOM_FALL_10 = "250_custom_fall_10";
    private string A_251_CUSTOM_LAND_1 = "251_custom_land_1";
    private string A_252_CUSTOM_LAND_2 = "252_custom_land_2";
    private string A_253_CUSTOM_LAND_3 = "253_custom_land_3";
    private string A_254_CUSTOM_LAND_4 = "254_custom_land_4";
    private string A_255_CUSTOM_LAND_5 = "255_custom_land_5";
    private string A_256_CUSTOM_LAND_6 = "256_custom_land_6";
    private string A_257_CUSTOM_LAND_7 = "257_custom_land_7";
    private string A_258_CUSTOM_LAND_8 = "258_custom_land_8";
    private string A_259_CUSTOM_LAND_9 = "259_custom_land_9";
    private string A_260_CUSTOM_LAND_10 = "260_custom_land_10";
    private string A_261_CUSTOM_PUNCH_1 = "261_custom_punch_1";
    private string A_262_CUSTOM_PUNCH_2 = "262_custom_punch_2";
    private string A_263_CUSTOM_PUNCH_3 = "263_custom_punch_3";
    private string A_264_CUSTOM_PUNCH_4 = "264_custom_punch_4";
    private string A_265_CUSTOM_PUNCH_5 = "265_custom_punch_5";
    private string A_266_CUSTOM_PUNCH_6 = "266_custom_punch_6";
    private string A_267_CUSTOM_PUNCH_7 = "267_custom_punch_7";
    private string A_268_CUSTOM_PUNCH_8 = "268_custom_punch_8";
    private string A_269_CUSTOM_PUNCH_9 = "269_custom_punch_9";
    private string A_270_CUSTOM_PUNCH_10 = "270_custom_punch_10";
    private string A_271_CUSTOM_KICK_1 = "271_custom_kick_1";
    private string A_272_CUSTOM_KICK_2 = "272_custom_kick_2";
    private string A_273_CUSTOM_KICK_3 = "273_custom_kick_3";
    private string A_274_CUSTOM_KICK_4 = "274_custom_kick_4";
    private string A_275_CUSTOM_KICK_5 = "275_custom_kick_5";
    private string A_276_CUSTOM_KICK_6 = "276_custom_kick_6";
    private string A_277_CUSTOM_KICK_7 = "277_custom_kick_7";
    private string A_278_CUSTOM_KICK_8 = "278_custom_kick_8";
    private string A_279_CUSTOM_KICK_9 = "279_custom_kick_9";
    private string A_280_CUSTOM_KICK_10 = "280_custom_kick_10";
    private string A_281_CUSTOM_ATTACK_1 = "281_custom_attack_1";
    private string A_282_CUSTOM_ATTACK_2 = "282_custom_attack_2";
    private string A_283_CUSTOM_ATTACK_3 = "283_custom_attack_3";
    private string A_284_CUSTOM_ATTACK_4 = "284_custom_attack_4";
    private string A_285_CUSTOM_ATTACK_5 = "285_custom_attack_5";
    private string A_286_CUSTOM_ATTACK_6 = "286_custom_attack_6";
    private string A_287_CUSTOM_ATTACK_7 = "287_custom_attack_7";
    private string A_288_CUSTOM_ATTACK_8 = "288_custom_attack_8";
    private string A_289_CUSTOM_ATTACK_9 = "289_custom_attack_9";
    private string A_290_CUSTOM_ATTACK_10 = "290_custom_attack_10";
    private string A_291_CUSTOM_MAGIC_1 = "291_custom_magic_1";
    private string A_292_CUSTOM_MAGIC_2 = "292_custom_magic_2";
    private string A_293_CUSTOM_MAGIC_3 = "293_custom_magic_3";
    private string A_294_CUSTOM_MAGIC_4 = "294_custom_magic_4";
    private string A_295_CUSTOM_MAGIC_5 = "295_custom_magic_5";
    private string A_296_CUSTOM_MAGIC_6 = "296_custom_magic_6";
    private string A_297_CUSTOM_MAGIC_7 = "297_custom_magic_7";
    private string A_298_CUSTOM_MAGIC_8 = "298_custom_magic_8";
    private string A_299_CUSTOM_MAGIC_9 = "299_custom_magic_9";
    private string A_300_CUSTOM_MAGIC_10 = "300_custom_magic_10";
    private string A_301_CUSTOM_BLOCK_1 = "301_custom_block_1";
    private string A_302_CUSTOM_BLOCK_2 = "302_custom_block_2";
    private string A_303_CUSTOM_BLOCK_3 = "303_custom_block_3";
    private string A_304_CUSTOM_BLOCK_4 = "304_custom_block_4";
    private string A_305_CUSTOM_BLOCK_5 = "305_custom_block_5";
    private string A_306_CUSTOM_BLOCK_6 = "306_custom_block_6";
    private string A_307_CUSTOM_BLOCK_7 = "307_custom_block_7";
    private string A_308_CUSTOM_BLOCK_8 = "308_custom_block_8";
    private string A_309_CUSTOM_BLOCK_9 = "309_custom_block_9";
    private string A_310_CUSTOM_BLOCK_10 = "310_custom_block_10";
    private string A_311_CUSTOM_HIT_1 = "311_custom_hit_1";
    private string A_312_CUSTOM_HIT_2 = "312_custom_hit_2";
    private string A_313_CUSTOM_HIT_3 = "313_custom_hit_3";
    private string A_314_CUSTOM_HIT_4 = "314_custom_hit_4";
    private string A_315_CUSTOM_HIT_5 = "315_custom_hit_5";
    private string A_316_CUSTOM_HIT_6 = "316_custom_hit_6";
    private string A_317_CUSTOM_HIT_7 = "317_custom_hit_7";
    private string A_318_CUSTOM_HIT_8 = "318_custom_hit_8";
    private string A_319_CUSTOM_HIT_9 = "319_custom_hit_9";
    private string A_320_CUSTOM_HIT_10 = "320_custom_hit_10";
    private string A_321_CUSTOM_LOSE_1 = "321_custom_lose_1";
    private string A_322_CUSTOM_LOSE_2 = "322_custom_lose_2";
    private string A_323_CUSTOM_LOSE_3 = "323_custom_lose_3";
    private string A_324_CUSTOM_LOSE_4 = "324_custom_lose_4";
    private string A_325_CUSTOM_LOSE_5 = "325_custom_lose_5";
    private string A_326_CUSTOM_LOSE_6 = "326_custom_lose_6";
    private string A_327_CUSTOM_LOSE_7 = "327_custom_lose_7";
    private string A_328_CUSTOM_LOSE_8 = "328_custom_lose_8";
    private string A_329_CUSTOM_LOSE_9 = "329_custom_lose_9";
    private string A_330_CUSTOM_LOSE_10 = "330_custom_lose_10";
    private string A_331_CUSTOM_DIE_1 = "331_custom_die_1";
    private string A_332_CUSTOM_DIE_2 = "332_custom_die_2";
    private string A_333_CUSTOM_DIE_3 = "333_custom_die_3";
    private string A_334_CUSTOM_DIE_4 = "334_custom_die_4";
    private string A_335_CUSTOM_DIE_5 = "335_custom_die_5";
    private string A_336_CUSTOM_DIE_6 = "336_custom_die_6";
    private string A_337_CUSTOM_DIE_7 = "337_custom_die_7";
    private string A_338_CUSTOM_DIE_8 = "338_custom_die_8";
    private string A_339_CUSTOM_DIE_9 = "339_custom_die_9";
    private string A_340_CUSTOM_DIE_10 = "340_custom_die_10";
    private string A_341_CUSTOM_VICTORY_1 = "341_custom_victory_1";
    private string A_342_CUSTOM_VICTORY_2 = "342_custom_victory_2";
    private string A_343_CUSTOM_VICTORY_3 = "343_custom_victory_3";
    private string A_344_CUSTOM_VICTORY_4 = "344_custom_victory_4";
    private string A_345_CUSTOM_VICTORY_5 = "345_custom_victory_5";
    private string A_346_CUSTOM_VICTORY_6 = "346_custom_victory_6";
    private string A_347_CUSTOM_VICTORY_7 = "347_custom_victory_7";
    private string A_348_CUSTOM_VICTORY_8 = "348_custom_victory_8";
    private string A_349_CUSTOM_VICTORY_9 = "349_custom_victory_9";
    private string A_350_CUSTOM_VICTORY_10 = "350_custom_victory_10";
    private string A_351_CUSTOM_EXTRA_ACTIONS_1 = "351_custom_extra_actions_1";
    private string A_352_CUSTOM_EXTRA_ACTIONS_2 = "352_custom_extra_actions_2";
    private string A_353_CUSTOM_EXTRA_ACTIONS_3 = "353_custom_extra_actions_3";
    private string A_354_CUSTOM_EXTRA_ACTIONS_4 = "354_custom_extra_actions_4";
    private string A_355_CUSTOM_EXTRA_ACTIONS_5 = "355_custom_extra_actions_5";
    private string A_356_CUSTOM_EXTRA_ACTIONS_6 = "356_custom_extra_actions_6";
    private string A_357_CUSTOM_EXTRA_ACTIONS_7 = "357_custom_extra_actions_7";
    private string A_358_CUSTOM_EXTRA_ACTIONS_8 = "358_custom_extra_actions_8";
    private string A_359_CUSTOM_EXTRA_ACTIONS_9 = "359_custom_extra_actions_9";
    private string A_360_CUSTOM_EXTRA_ACTIONS_10 = "360_custom_extra_actions_10";

    [Header("Actions ID")]
    private int A_001_POSE_1_ID = 1;

    private int A_002_POSE_2_ID = 2;
    private int A_003_POSE_3_ID = 3;
    private int A_004_POSE_4_ID = 4;
    private int A_005_POSE_5_ID = 5;
    private int A_006_POSE_6_ID = 6;
    private int A_007_POSE_7_ID = 7;
    private int A_008_POSE_8_ID = 8;
    private int A_009_POSE_9_ID = 9;
    private int A_010_POSE_10_ID = 10;
    private int A_011_IDLE_1_ID = 11;
    private int A_012_IDLE_2_ID = 12;
    private int A_013_IDLE_3_ID = 13;
    private int A_014_IDLE_4_ID = 14;
    private int A_015_IDLE_5_ID = 15;
    private int A_016_IDLE_6_ID = 16;
    private int A_017_IDLE_7_ID = 17;
    private int A_018_IDLE_8_ID = 18;
    private int A_019_IDLE_9_ID = 19;
    private int A_020_IDLE_10_ID = 20;
    private int A_021_WALK_1_ID = 21;
    private int A_022_WALK_2_ID = 22;
    private int A_023_WALK_3_ID = 23;
    private int A_024_WALK_4_ID = 24;
    private int A_025_WALK_5_ID = 25;
    private int A_026_WALK_6_ID = 26;
    private int A_027_WALK_7_ID = 27;
    private int A_028_WALK_8_ID = 28;
    private int A_029_WALK_9_ID = 29;
    private int A_030_WALK_10_ID = 30;
    private int A_031_RUN_1_ID = 31;
    private int A_032_RUN_2_ID = 32;
    private int A_033_RUN_3_ID = 33;
    private int A_034_RUN_4_ID = 34;
    private int A_035_RUN_5_ID = 35;
    private int A_036_RUN_6_ID = 36;
    private int A_037_RUN_7_ID = 37;
    private int A_038_RUN_8_ID = 38;
    private int A_039_RUN_9_ID = 39;
    private int A_040_RUN_10_ID = 40;
    private int A_041_JUMP_1_ID = 41;
    private int A_042_JUMP_2_ID = 42;
    private int A_043_JUMP_3_ID = 43;
    private int A_044_JUMP_4_ID = 44;
    private int A_045_JUMP_5_ID = 45;
    private int A_046_JUMP_6_ID = 46;
    private int A_047_JUMP_7_ID = 47;
    private int A_048_JUMP_8_ID = 48;
    private int A_049_JUMP_9_ID = 49;
    private int A_050_JUMP_10_ID = 50;
    private int A_051_JUMP_SPIN_1_ID = 51;
    private int A_052_JUMP_SPIN_2_ID = 52;
    private int A_053_JUMP_SPIN_3_ID = 53;
    private int A_054_JUMP_SPIN_4_ID = 54;
    private int A_055_JUMP_SPIN_5_ID = 55;
    private int A_056_JUMP_SPIN_6_ID = 56;
    private int A_057_JUMP_SPIN_7_ID = 57;
    private int A_058_JUMP_SPIN_8_ID = 58;
    private int A_059_JUMP_SPIN_9_ID = 59;
    private int A_060_JUMP_SPIN_10_ID = 60;
    private int A_061_FALL_1_ID = 61;
    private int A_062_FALL_2_ID = 62;
    private int A_063_FALL_3_ID = 63;
    private int A_064_FALL_4_ID = 64;
    private int A_065_FALL_5_ID = 65;
    private int A_066_FALL_6_ID = 66;
    private int A_067_FALL_7_ID = 67;
    private int A_068_FALL_8_ID = 68;
    private int A_069_FALL_9_ID = 69;
    private int A_070_FALL_10_ID = 70;
    private int A_071_LAND_1_ID = 71;
    private int A_072_LAND_2_ID = 72;
    private int A_073_LAND_3_ID = 73;
    private int A_074_LAND_4_ID = 74;
    private int A_075_LAND_5_ID = 75;
    private int A_076_LAND_6_ID = 76;
    private int A_077_LAND_7_ID = 77;
    private int A_078_LAND_8_ID = 78;
    private int A_079_LAND_9_ID = 79;
    private int A_080_LAND_10_ID = 80;
    private int A_081_PUNCH_1_ID = 81;
    private int A_082_PUNCH_2_ID = 82;
    private int A_083_PUNCH_3_ID = 83;
    private int A_084_PUNCH_4_ID = 84;
    private int A_085_PUNCH_5_ID = 85;
    private int A_086_PUNCH_6_ID = 86;
    private int A_087_PUNCH_7_ID = 87;
    private int A_088_PUNCH_8_ID = 88;
    private int A_089_PUNCH_9_ID = 89;
    private int A_090_PUNCH_10_ID = 90;
    private int A_091_KICK_1_ID = 91;
    private int A_092_KICK_2_ID = 92;
    private int A_093_KICK_3_ID = 93;
    private int A_094_KICK_4_ID = 94;
    private int A_095_KICK_5_ID = 95;
    private int A_096_KICK_6_ID = 96;
    private int A_097_KICK_7_ID = 97;
    private int A_098_KICK_8_ID = 98;
    private int A_099_KICK_9_ID = 99;
    private int A_100_KICK_10_ID = 100;
    private int A_101_ATTACK_1_ID = 101;
    private int A_102_ATTACK_2_ID = 102;
    private int A_103_ATTACK_3_ID = 103;
    private int A_104_ATTACK_4_ID = 104;
    private int A_105_ATTACK_5_ID = 105;
    private int A_106_ATTACK_6_ID = 106;
    private int A_107_ATTACK_7_ID = 107;
    private int A_108_ATTACK_8_ID = 108;
    private int A_109_ATTACK_9_ID = 109;
    private int A_110_ATTACK_10_ID = 110;
    private int A_111_MAGIC_1_ID = 111;
    private int A_112_MAGIC_2_ID = 112;
    private int A_113_MAGIC_3_ID = 113;
    private int A_114_MAGIC_4_ID = 114;
    private int A_115_MAGIC_5_ID = 115;
    private int A_116_MAGIC_6_ID = 116;
    private int A_117_MAGIC_7_ID = 117;
    private int A_118_MAGIC_8_ID = 118;
    private int A_119_MAGIC_9_ID = 119;
    private int A_120_MAGIC_10_ID = 120;
    private int A_121_BLOCK_1_ID = 121;
    private int A_122_BLOCK_2_ID = 122;
    private int A_123_BLOCK_3_ID = 123;
    private int A_124_BLOCK_4_ID = 124;
    private int A_125_BLOCK_5_ID = 125;
    private int A_126_BLOCK_6_ID = 126;
    private int A_127_BLOCK_7_ID = 127;
    private int A_128_BLOCK_8_ID = 128;
    private int A_129_BLOCK_9_ID = 129;
    private int A_130_BLOCK_10_ID = 130;
    private int A_131_HIT_1_ID = 131;
    private int A_132_HIT_2_ID = 132;
    private int A_133_HIT_3_ID = 133;
    private int A_134_HIT_4_ID = 134;
    private int A_135_HIT_5_ID = 135;
    private int A_136_HIT_6_ID = 136;
    private int A_137_HIT_7_ID = 137;
    private int A_138_HIT_8_ID = 138;
    private int A_139_HIT_9_ID = 139;
    private int A_140_HIT_10_ID = 140;
    private int A_141_LOSE_1_ID = 141;
    private int A_142_LOSE_2_ID = 142;
    private int A_143_LOSE_3_ID = 143;
    private int A_144_LOSE_4_ID = 144;
    private int A_145_LOSE_5_ID = 145;
    private int A_146_LOSE_6_ID = 146;
    private int A_147_LOSE_7_ID = 147;
    private int A_148_LOSE_8_ID = 148;
    private int A_149_LOSE_9_ID = 149;
    private int A_150_LOSE_10_ID = 150;
    private int A_151_DIE_1_ID = 151;
    private int A_152_DIE_2_ID = 152;
    private int A_153_DIE_3_ID = 153;
    private int A_154_DIE_4_ID = 154;
    private int A_155_DIE_5_ID = 155;
    private int A_156_DIE_6_ID = 156;
    private int A_157_DIE_7_ID = 157;
    private int A_158_DIE_8_ID = 158;
    private int A_159_DIE_9_ID = 159;
    private int A_160_DIE_10_ID = 160;
    private int A_161_VICTORY_1_ID = 161;
    private int A_162_VICTORY_2_ID = 162;
    private int A_163_VICTORY_3_ID = 163;
    private int A_164_VICTORY_4_ID = 164;
    private int A_165_VICTORY_5_ID = 165;
    private int A_166_VICTORY_6_ID = 166;
    private int A_167_VICTORY_7_ID = 167;
    private int A_168_VICTORY_8_ID = 168;
    private int A_169_VICTORY_9_ID = 169;
    private int A_170_VICTORY_10_ID = 170;
    private int A_171_EXTRA_ACTIONS_1_ID = 171;
    private int A_172_EXTRA_ACTIONS_2_ID = 172;
    private int A_173_EXTRA_ACTIONS_3_ID = 173;
    private int A_174_EXTRA_ACTIONS_4_ID = 174;
    private int A_175_EXTRA_ACTIONS_5_ID = 175;
    private int A_176_EXTRA_ACTIONS_6_ID = 176;
    private int A_177_EXTRA_ACTIONS_7_ID = 177;
    private int A_178_EXTRA_ACTIONS_8_ID = 178;
    private int A_179_EXTRA_ACTIONS_9_ID = 179;
    private int A_180_EXTRA_ACTIONS_10_ID = 180;
    private int A_181_CUSTOM_POSE_1_ID = 181;
    private int A_182_CUSTOM_POSE_2_ID = 182;
    private int A_183_CUSTOM_POSE_3_ID = 183;
    private int A_184_CUSTOM_POSE_4_ID = 184;
    private int A_185_CUSTOM_POSE_5_ID = 185;
    private int A_186_CUSTOM_POSE_6_ID = 186;
    private int A_187_CUSTOM_POSE_7_ID = 187;
    private int A_188_CUSTOM_POSE_8_ID = 188;
    private int A_189_CUSTOM_POSE_9_ID = 189;
    private int A_190_CUSTOM_POSE_10_ID = 190;
    private int A_191_CUSTOM_IDLE_1_ID = 191;
    private int A_192_CUSTOM_IDLE_2_ID = 192;
    private int A_193_CUSTOM_IDLE_3_ID = 193;
    private int A_194_CUSTOM_IDLE_4_ID = 194;
    private int A_195_CUSTOM_IDLE_5_ID = 195;
    private int A_196_CUSTOM_IDLE_6_ID = 196;
    private int A_197_CUSTOM_IDLE_7_ID = 197;
    private int A_198_CUSTOM_IDLE_8_ID = 198;
    private int A_199_CUSTOM_IDLE_9_ID = 199;
    private int A_200_CUSTOM_IDLE_10_ID = 200;
    private int A_201_CUSTOM_WALK_1_ID = 201;
    private int A_202_CUSTOM_WALK_2_ID = 202;
    private int A_203_CUSTOM_WALK_3_ID = 203;
    private int A_204_CUSTOM_WALK_4_ID = 204;
    private int A_205_CUSTOM_WALK_5_ID = 205;
    private int A_206_CUSTOM_WALK_6_ID = 206;
    private int A_207_CUSTOM_WALK_7_ID = 207;
    private int A_208_CUSTOM_WALK_8_ID = 208;
    private int A_209_CUSTOM_WALK_9_ID = 209;
    private int A_210_CUSTOM_WALK_10_ID = 210;
    private int A_211_CUSTOM_RUN_1_ID = 211;
    private int A_212_CUSTOM_RUN_2_ID = 212;
    private int A_213_CUSTOM_RUN_3_ID = 213;
    private int A_214_CUSTOM_RUN_4_ID = 214;
    private int A_215_CUSTOM_RUN_5_ID = 215;
    private int A_216_CUSTOM_RUN_6_ID = 216;
    private int A_217_CUSTOM_RUN_7_ID = 217;
    private int A_218_CUSTOM_RUN_8_ID = 218;
    private int A_219_CUSTOM_RUN_9_ID = 219;
    private int A_220_CUSTOM_RUN_10_ID = 220;
    private int A_221_CUSTOM_JUMP_1_ID = 221;
    private int A_222_CUSTOM_JUMP_2_ID = 222;
    private int A_223_CUSTOM_JUMP_3_ID = 223;
    private int A_224_CUSTOM_JUMP_4_ID = 224;
    private int A_225_CUSTOM_JUMP_5_ID = 225;
    private int A_226_CUSTOM_JUMP_6_ID = 226;
    private int A_227_CUSTOM_JUMP_7_ID = 227;
    private int A_228_CUSTOM_JUMP_8_ID = 228;
    private int A_229_CUSTOM_JUMP_9_ID = 229;
    private int A_230_CUSTOM_JUMP_10_ID = 230;
    private int A_231_CUSTOM_JUMP_SPIN_1_ID = 231;
    private int A_232_CUSTOM_JUMP_SPIN_2_ID = 232;
    private int A_233_CUSTOM_JUMP_SPIN_3_ID = 233;
    private int A_234_CUSTOM_JUMP_SPIN_4_ID = 234;
    private int A_235_CUSTOM_JUMP_SPIN_5_ID = 235;
    private int A_236_CUSTOM_JUMP_SPIN_6_ID = 236;
    private int A_237_CUSTOM_JUMP_SPIN_7_ID = 237;
    private int A_238_CUSTOM_JUMP_SPIN_8_ID = 238;
    private int A_239_CUSTOM_JUMP_SPIN_9_ID = 239;
    private int A_240_CUSTOM_JUMP_SPIN_10_ID = 240;
    private int A_241_CUSTOM_FALL_1_ID = 241;
    private int A_242_CUSTOM_FALL_2_ID = 242;
    private int A_243_CUSTOM_FALL_3_ID = 243;
    private int A_244_CUSTOM_FALL_4_ID = 244;
    private int A_245_CUSTOM_FALL_5_ID = 245;
    private int A_246_CUSTOM_FALL_6_ID = 246;
    private int A_247_CUSTOM_FALL_7_ID = 247;
    private int A_248_CUSTOM_FALL_8_ID = 248;
    private int A_249_CUSTOM_FALL_9_ID = 249;
    private int A_250_CUSTOM_FALL_10_ID = 250;
    private int A_251_CUSTOM_LAND_1_ID = 251;
    private int A_252_CUSTOM_LAND_2_ID = 252;
    private int A_253_CUSTOM_LAND_3_ID = 253;
    private int A_254_CUSTOM_LAND_4_ID = 254;
    private int A_255_CUSTOM_LAND_5_ID = 255;
    private int A_256_CUSTOM_LAND_6_ID = 256;
    private int A_257_CUSTOM_LAND_7_ID = 257;
    private int A_258_CUSTOM_LAND_8_ID = 258;
    private int A_259_CUSTOM_LAND_9_ID = 259;
    private int A_260_CUSTOM_LAND_10_ID = 260;
    private int A_261_CUSTOM_PUNCH_1_ID = 261;
    private int A_262_CUSTOM_PUNCH_2_ID = 262;
    private int A_263_CUSTOM_PUNCH_3_ID = 263;
    private int A_264_CUSTOM_PUNCH_4_ID = 264;
    private int A_265_CUSTOM_PUNCH_5_ID = 265;
    private int A_266_CUSTOM_PUNCH_6_ID = 266;
    private int A_267_CUSTOM_PUNCH_7_ID = 267;
    private int A_268_CUSTOM_PUNCH_8_ID = 268;
    private int A_269_CUSTOM_PUNCH_9_ID = 269;
    private int A_270_CUSTOM_PUNCH_10_ID = 270;
    private int A_271_CUSTOM_KICK_1_ID = 271;
    private int A_272_CUSTOM_KICK_2_ID = 272;
    private int A_273_CUSTOM_KICK_3_ID = 273;
    private int A_274_CUSTOM_KICK_4_ID = 274;
    private int A_275_CUSTOM_KICK_5_ID = 275;
    private int A_276_CUSTOM_KICK_6_ID = 276;
    private int A_277_CUSTOM_KICK_7_ID = 277;
    private int A_278_CUSTOM_KICK_8_ID = 278;
    private int A_279_CUSTOM_KICK_9_ID = 279;
    private int A_280_CUSTOM_KICK_10_ID = 280;
    private int A_281_CUSTOM_ATTACK_1_ID = 281;
    private int A_282_CUSTOM_ATTACK_2_ID = 282;
    private int A_283_CUSTOM_ATTACK_3_ID = 283;
    private int A_284_CUSTOM_ATTACK_4_ID = 284;
    private int A_285_CUSTOM_ATTACK_5_ID = 285;
    private int A_286_CUSTOM_ATTACK_6_ID = 286;
    private int A_287_CUSTOM_ATTACK_7_ID = 287;
    private int A_288_CUSTOM_ATTACK_8_ID = 288;
    private int A_289_CUSTOM_ATTACK_9_ID = 289;
    private int A_290_CUSTOM_ATTACK_10_ID = 290;
    private int A_291_CUSTOM_MAGIC_1_ID = 291;
    private int A_292_CUSTOM_MAGIC_2_ID = 292;
    private int A_293_CUSTOM_MAGIC_3_ID = 293;
    private int A_294_CUSTOM_MAGIC_4_ID = 294;
    private int A_295_CUSTOM_MAGIC_5_ID = 295;
    private int A_296_CUSTOM_MAGIC_6_ID = 296;
    private int A_297_CUSTOM_MAGIC_7_ID = 297;
    private int A_298_CUSTOM_MAGIC_8_ID = 298;
    private int A_299_CUSTOM_MAGIC_9_ID = 299;
    private int A_300_CUSTOM_MAGIC_10_ID = 300;
    private int A_301_CUSTOM_BLOCK_1_ID = 301;
    private int A_302_CUSTOM_BLOCK_2_ID = 302;
    private int A_303_CUSTOM_BLOCK_3_ID = 303;
    private int A_304_CUSTOM_BLOCK_4_ID = 304;
    private int A_305_CUSTOM_BLOCK_5_ID = 305;
    private int A_306_CUSTOM_BLOCK_6_ID = 306;
    private int A_307_CUSTOM_BLOCK_7_ID = 307;
    private int A_308_CUSTOM_BLOCK_8_ID = 308;
    private int A_309_CUSTOM_BLOCK_9_ID = 309;
    private int A_310_CUSTOM_BLOCK_10_ID = 310;
    private int A_311_CUSTOM_HIT_1_ID = 311;
    private int A_312_CUSTOM_HIT_2_ID = 312;
    private int A_313_CUSTOM_HIT_3_ID = 313;
    private int A_314_CUSTOM_HIT_4_ID = 314;
    private int A_315_CUSTOM_HIT_5_ID = 315;
    private int A_316_CUSTOM_HIT_6_ID = 316;
    private int A_317_CUSTOM_HIT_7_ID = 317;
    private int A_318_CUSTOM_HIT_8_ID = 318;
    private int A_319_CUSTOM_HIT_9_ID = 319;
    private int A_320_CUSTOM_HIT_10_ID = 320;
    private int A_321_CUSTOM_LOSE_1_ID = 321;
    private int A_322_CUSTOM_LOSE_2_ID = 322;
    private int A_323_CUSTOM_LOSE_3_ID = 323;
    private int A_324_CUSTOM_LOSE_4_ID = 324;
    private int A_325_CUSTOM_LOSE_5_ID = 325;
    private int A_326_CUSTOM_LOSE_6_ID = 326;
    private int A_327_CUSTOM_LOSE_7_ID = 327;
    private int A_328_CUSTOM_LOSE_8_ID = 328;
    private int A_329_CUSTOM_LOSE_9_ID = 329;
    private int A_330_CUSTOM_LOSE_10_ID = 330;
    private int A_331_CUSTOM_DIE_1_ID = 331;
    private int A_332_CUSTOM_DIE_2_ID = 332;
    private int A_333_CUSTOM_DIE_3_ID = 333;
    private int A_334_CUSTOM_DIE_4_ID = 334;
    private int A_335_CUSTOM_DIE_5_ID = 335;
    private int A_336_CUSTOM_DIE_6_ID = 336;
    private int A_337_CUSTOM_DIE_7_ID = 337;
    private int A_338_CUSTOM_DIE_8_ID = 338;
    private int A_339_CUSTOM_DIE_9_ID = 339;
    private int A_340_CUSTOM_DIE_10_ID = 340;
    private int A_341_CUSTOM_VICTORY_1_ID = 341;
    private int A_342_CUSTOM_VICTORY_2_ID = 342;
    private int A_343_CUSTOM_VICTORY_3_ID = 343;
    private int A_344_CUSTOM_VICTORY_4_ID = 344;
    private int A_345_CUSTOM_VICTORY_5_ID = 345;
    private int A_346_CUSTOM_VICTORY_6_ID = 346;
    private int A_347_CUSTOM_VICTORY_7_ID = 347;
    private int A_348_CUSTOM_VICTORY_8_ID = 348;
    private int A_349_CUSTOM_VICTORY_9_ID = 349;
    private int A_350_CUSTOM_VICTORY_10_ID = 350;
    private int A_351_CUSTOM_EXTRA_ACTIONS_1_ID = 351;
    private int A_352_CUSTOM_EXTRA_ACTIONS_2_ID = 352;
    private int A_353_CUSTOM_EXTRA_ACTIONS_3_ID = 353;
    private int A_354_CUSTOM_EXTRA_ACTIONS_4_ID = 354;
    private int A_355_CUSTOM_EXTRA_ACTIONS_5_ID = 355;
    private int A_356_CUSTOM_EXTRA_ACTIONS_6_ID = 356;
    private int A_357_CUSTOM_EXTRA_ACTIONS_7_ID = 357;
    private int A_358_CUSTOM_EXTRA_ACTIONS_8_ID = 358;
    private int A_359_CUSTOM_EXTRA_ACTIONS_9_ID = 359;
    private int A_360_CUSTOM_EXTRA_ACTIONS_10_ID = 360;
    private string backActionName = "011_idle_1";
    private int backActionID = 11;

    private void Awake()
    {
        //gameObject.transform.position = new Vector3(0, 5, 0);
        FindComponents();
        actions[A_001_POSE_1] = A_001_POSE_1_ID;
        actions[A_002_POSE_2] = A_002_POSE_2_ID;
        actions[A_003_POSE_3] = A_003_POSE_3_ID;
        actions[A_004_POSE_4] = A_004_POSE_4_ID;
        actions[A_005_POSE_5] = A_005_POSE_5_ID;
        actions[A_006_POSE_6] = A_006_POSE_6_ID;
        actions[A_007_POSE_7] = A_007_POSE_7_ID;
        actions[A_008_POSE_8] = A_008_POSE_8_ID;
        actions[A_009_POSE_9] = A_009_POSE_9_ID;
        actions[A_010_POSE_10] = A_010_POSE_10_ID;
        actions[A_011_IDLE_1] = A_011_IDLE_1_ID;
        actions[A_012_IDLE_2] = A_012_IDLE_2_ID;
        actions[A_013_IDLE_3] = A_013_IDLE_3_ID;
        actions[A_014_IDLE_4] = A_014_IDLE_4_ID;
        actions[A_015_IDLE_5] = A_015_IDLE_5_ID;
        actions[A_016_IDLE_6] = A_016_IDLE_6_ID;
        actions[A_017_IDLE_7] = A_017_IDLE_7_ID;
        actions[A_018_IDLE_8] = A_018_IDLE_8_ID;
        actions[A_019_IDLE_9] = A_019_IDLE_9_ID;
        actions[A_020_IDLE_10] = A_020_IDLE_10_ID;
        actions[A_021_WALK_1] = A_021_WALK_1_ID;
        actions[A_022_WALK_2] = A_022_WALK_2_ID;
        actions[A_023_WALK_3] = A_023_WALK_3_ID;
        actions[A_024_WALK_4] = A_024_WALK_4_ID;
        actions[A_025_WALK_5] = A_025_WALK_5_ID;
        actions[A_026_WALK_6] = A_026_WALK_6_ID;
        actions[A_027_WALK_7] = A_027_WALK_7_ID;
        actions[A_028_WALK_8] = A_028_WALK_8_ID;
        actions[A_029_WALK_9] = A_029_WALK_9_ID;
        actions[A_030_WALK_10] = A_030_WALK_10_ID;
        actions[A_031_RUN_1] = A_031_RUN_1_ID;
        actions[A_032_RUN_2] = A_032_RUN_2_ID;
        actions[A_033_RUN_3] = A_033_RUN_3_ID;
        actions[A_034_RUN_4] = A_034_RUN_4_ID;
        actions[A_035_RUN_5] = A_035_RUN_5_ID;
        actions[A_036_RUN_6] = A_036_RUN_6_ID;
        actions[A_037_RUN_7] = A_037_RUN_7_ID;
        actions[A_038_RUN_8] = A_038_RUN_8_ID;
        actions[A_039_RUN_9] = A_039_RUN_9_ID;
        actions[A_040_RUN_10] = A_040_RUN_10_ID;
        actions[A_041_JUMP_1] = A_041_JUMP_1_ID;
        actions[A_042_JUMP_2] = A_042_JUMP_2_ID;
        actions[A_043_JUMP_3] = A_043_JUMP_3_ID;
        actions[A_044_JUMP_4] = A_044_JUMP_4_ID;
        actions[A_045_JUMP_5] = A_045_JUMP_5_ID;
        actions[A_046_JUMP_6] = A_046_JUMP_6_ID;
        actions[A_047_JUMP_7] = A_047_JUMP_7_ID;
        actions[A_048_JUMP_8] = A_048_JUMP_8_ID;
        actions[A_049_JUMP_9] = A_049_JUMP_9_ID;
        actions[A_050_JUMP_10] = A_050_JUMP_10_ID;
        actions[A_051_JUMP_SPIN_1] = A_051_JUMP_SPIN_1_ID;
        actions[A_052_JUMP_SPIN_2] = A_052_JUMP_SPIN_2_ID;
        actions[A_053_JUMP_SPIN_3] = A_053_JUMP_SPIN_3_ID;
        actions[A_054_JUMP_SPIN_4] = A_054_JUMP_SPIN_4_ID;
        actions[A_055_JUMP_SPIN_5] = A_055_JUMP_SPIN_5_ID;
        actions[A_056_JUMP_SPIN_6] = A_056_JUMP_SPIN_6_ID;
        actions[A_057_JUMP_SPIN_7] = A_057_JUMP_SPIN_7_ID;
        actions[A_058_JUMP_SPIN_8] = A_058_JUMP_SPIN_8_ID;
        actions[A_059_JUMP_SPIN_9] = A_059_JUMP_SPIN_9_ID;
        actions[A_060_JUMP_SPIN_10] = A_060_JUMP_SPIN_10_ID;
        actions[A_061_FALL_1] = A_061_FALL_1_ID;
        actions[A_062_FALL_2] = A_062_FALL_2_ID;
        actions[A_063_FALL_3] = A_063_FALL_3_ID;
        actions[A_064_FALL_4] = A_064_FALL_4_ID;
        actions[A_065_FALL_5] = A_065_FALL_5_ID;
        actions[A_066_FALL_6] = A_066_FALL_6_ID;
        actions[A_067_FALL_7] = A_067_FALL_7_ID;
        actions[A_068_FALL_8] = A_068_FALL_8_ID;
        actions[A_069_FALL_9] = A_069_FALL_9_ID;
        actions[A_070_FALL_10] = A_070_FALL_10_ID;
        actions[A_071_LAND_1] = A_071_LAND_1_ID;
        actions[A_072_LAND_2] = A_072_LAND_2_ID;
        actions[A_073_LAND_3] = A_073_LAND_3_ID;
        actions[A_074_LAND_4] = A_074_LAND_4_ID;
        actions[A_075_LAND_5] = A_075_LAND_5_ID;
        actions[A_076_LAND_6] = A_076_LAND_6_ID;
        actions[A_077_LAND_7] = A_077_LAND_7_ID;
        actions[A_078_LAND_8] = A_078_LAND_8_ID;
        actions[A_079_LAND_9] = A_079_LAND_9_ID;
        actions[A_080_LAND_10] = A_080_LAND_10_ID;
        actions[A_081_PUNCH_1] = A_081_PUNCH_1_ID;
        actions[A_082_PUNCH_2] = A_082_PUNCH_2_ID;
        actions[A_083_PUNCH_3] = A_083_PUNCH_3_ID;
        actions[A_084_PUNCH_4] = A_084_PUNCH_4_ID;
        actions[A_085_PUNCH_5] = A_085_PUNCH_5_ID;
        actions[A_086_PUNCH_6] = A_086_PUNCH_6_ID;
        actions[A_087_PUNCH_7] = A_087_PUNCH_7_ID;
        actions[A_088_PUNCH_8] = A_088_PUNCH_8_ID;
        actions[A_089_PUNCH_9] = A_089_PUNCH_9_ID;
        actions[A_090_PUNCH_10] = A_090_PUNCH_10_ID;
        actions[A_091_KICK_1] = A_091_KICK_1_ID;
        actions[A_092_KICK_2] = A_092_KICK_2_ID;
        actions[A_093_KICK_3] = A_093_KICK_3_ID;
        actions[A_094_KICK_4] = A_094_KICK_4_ID;
        actions[A_095_KICK_5] = A_095_KICK_5_ID;
        actions[A_096_KICK_6] = A_096_KICK_6_ID;
        actions[A_097_KICK_7] = A_097_KICK_7_ID;
        actions[A_098_KICK_8] = A_098_KICK_8_ID;
        actions[A_099_KICK_9] = A_099_KICK_9_ID;
        actions[A_100_KICK_10] = A_100_KICK_10_ID;
        actions[A_101_ATTACK_1] = A_101_ATTACK_1_ID;
        actions[A_102_ATTACK_2] = A_102_ATTACK_2_ID;
        actions[A_103_ATTACK_3] = A_103_ATTACK_3_ID;
        actions[A_104_ATTACK_4] = A_104_ATTACK_4_ID;
        actions[A_105_ATTACK_5] = A_105_ATTACK_5_ID;
        actions[A_106_ATTACK_6] = A_106_ATTACK_6_ID;
        actions[A_107_ATTACK_7] = A_107_ATTACK_7_ID;
        actions[A_108_ATTACK_8] = A_108_ATTACK_8_ID;
        actions[A_109_ATTACK_9] = A_109_ATTACK_9_ID;
        actions[A_110_ATTACK_10] = A_110_ATTACK_10_ID;
        actions[A_111_MAGIC_1] = A_111_MAGIC_1_ID;
        actions[A_112_MAGIC_2] = A_112_MAGIC_2_ID;
        actions[A_113_MAGIC_3] = A_113_MAGIC_3_ID;
        actions[A_114_MAGIC_4] = A_114_MAGIC_4_ID;
        actions[A_115_MAGIC_5] = A_115_MAGIC_5_ID;
        actions[A_116_MAGIC_6] = A_116_MAGIC_6_ID;
        actions[A_117_MAGIC_7] = A_117_MAGIC_7_ID;
        actions[A_118_MAGIC_8] = A_118_MAGIC_8_ID;
        actions[A_119_MAGIC_9] = A_119_MAGIC_9_ID;
        actions[A_120_MAGIC_10] = A_120_MAGIC_10_ID;
        actions[A_121_BLOCK_1] = A_121_BLOCK_1_ID;
        actions[A_122_BLOCK_2] = A_122_BLOCK_2_ID;
        actions[A_123_BLOCK_3] = A_123_BLOCK_3_ID;
        actions[A_124_BLOCK_4] = A_124_BLOCK_4_ID;
        actions[A_125_BLOCK_5] = A_125_BLOCK_5_ID;
        actions[A_126_BLOCK_6] = A_126_BLOCK_6_ID;
        actions[A_127_BLOCK_7] = A_127_BLOCK_7_ID;
        actions[A_128_BLOCK_8] = A_128_BLOCK_8_ID;
        actions[A_129_BLOCK_9] = A_129_BLOCK_9_ID;
        actions[A_130_BLOCK_10] = A_130_BLOCK_10_ID;
        actions[A_131_HIT_1] = A_131_HIT_1_ID;
        actions[A_132_HIT_2] = A_132_HIT_2_ID;
        actions[A_133_HIT_3] = A_133_HIT_3_ID;
        actions[A_134_HIT_4] = A_134_HIT_4_ID;
        actions[A_135_HIT_5] = A_135_HIT_5_ID;
        actions[A_136_HIT_6] = A_136_HIT_6_ID;
        actions[A_137_HIT_7] = A_137_HIT_7_ID;
        actions[A_138_HIT_8] = A_138_HIT_8_ID;
        actions[A_139_HIT_9] = A_139_HIT_9_ID;
        actions[A_140_HIT_10] = A_140_HIT_10_ID;
        actions[A_141_LOSE_1] = A_141_LOSE_1_ID;
        actions[A_142_LOSE_2] = A_142_LOSE_2_ID;
        actions[A_143_LOSE_3] = A_143_LOSE_3_ID;
        actions[A_144_LOSE_4] = A_144_LOSE_4_ID;
        actions[A_145_LOSE_5] = A_145_LOSE_5_ID;
        actions[A_146_LOSE_6] = A_146_LOSE_6_ID;
        actions[A_147_LOSE_7] = A_147_LOSE_7_ID;
        actions[A_148_LOSE_8] = A_148_LOSE_8_ID;
        actions[A_149_LOSE_9] = A_149_LOSE_9_ID;
        actions[A_150_LOSE_10] = A_150_LOSE_10_ID;
        actions[A_151_DIE_1] = A_151_DIE_1_ID;
        actions[A_152_DIE_2] = A_152_DIE_2_ID;
        actions[A_153_DIE_3] = A_153_DIE_3_ID;
        actions[A_154_DIE_4] = A_154_DIE_4_ID;
        actions[A_155_DIE_5] = A_155_DIE_5_ID;
        actions[A_156_DIE_6] = A_156_DIE_6_ID;
        actions[A_157_DIE_7] = A_157_DIE_7_ID;
        actions[A_158_DIE_8] = A_158_DIE_8_ID;
        actions[A_159_DIE_9] = A_159_DIE_9_ID;
        actions[A_160_DIE_10] = A_160_DIE_10_ID;
        actions[A_161_VICTORY_1] = A_161_VICTORY_1_ID;
        actions[A_162_VICTORY_2] = A_162_VICTORY_2_ID;
        actions[A_163_VICTORY_3] = A_163_VICTORY_3_ID;
        actions[A_164_VICTORY_4] = A_164_VICTORY_4_ID;
        actions[A_165_VICTORY_5] = A_165_VICTORY_5_ID;
        actions[A_166_VICTORY_6] = A_166_VICTORY_6_ID;
        actions[A_167_VICTORY_7] = A_167_VICTORY_7_ID;
        actions[A_168_VICTORY_8] = A_168_VICTORY_8_ID;
        actions[A_169_VICTORY_9] = A_169_VICTORY_9_ID;
        actions[A_170_VICTORY_10] = A_170_VICTORY_10_ID;
        actions[A_171_EXTRA_ACTIONS_1] = A_171_EXTRA_ACTIONS_1_ID;
        actions[A_172_EXTRA_ACTIONS_2] = A_172_EXTRA_ACTIONS_2_ID;
        actions[A_173_EXTRA_ACTIONS_3] = A_173_EXTRA_ACTIONS_3_ID;
        actions[A_174_EXTRA_ACTIONS_4] = A_174_EXTRA_ACTIONS_4_ID;
        actions[A_175_EXTRA_ACTIONS_5] = A_175_EXTRA_ACTIONS_5_ID;
        actions[A_176_EXTRA_ACTIONS_6] = A_176_EXTRA_ACTIONS_6_ID;
        actions[A_177_EXTRA_ACTIONS_7] = A_177_EXTRA_ACTIONS_7_ID;
        actions[A_178_EXTRA_ACTIONS_8] = A_178_EXTRA_ACTIONS_8_ID;
        actions[A_179_EXTRA_ACTIONS_9] = A_179_EXTRA_ACTIONS_9_ID;
        actions[A_180_EXTRA_ACTIONS_10] = A_180_EXTRA_ACTIONS_10_ID;
        actions[A_181_CUSTOM_POSE_1] = A_181_CUSTOM_POSE_1_ID;
        actions[A_182_CUSTOM_POSE_2] = A_182_CUSTOM_POSE_2_ID;
        actions[A_183_CUSTOM_POSE_3] = A_183_CUSTOM_POSE_3_ID;
        actions[A_184_CUSTOM_POSE_4] = A_184_CUSTOM_POSE_4_ID;
        actions[A_185_CUSTOM_POSE_5] = A_185_CUSTOM_POSE_5_ID;
        actions[A_186_CUSTOM_POSE_6] = A_186_CUSTOM_POSE_6_ID;
        actions[A_187_CUSTOM_POSE_7] = A_187_CUSTOM_POSE_7_ID;
        actions[A_188_CUSTOM_POSE_8] = A_188_CUSTOM_POSE_8_ID;
        actions[A_189_CUSTOM_POSE_9] = A_189_CUSTOM_POSE_9_ID;
        actions[A_190_CUSTOM_POSE_10] = A_190_CUSTOM_POSE_10_ID;
        actions[A_191_CUSTOM_IDLE_1] = A_191_CUSTOM_IDLE_1_ID;
        actions[A_192_CUSTOM_IDLE_2] = A_192_CUSTOM_IDLE_2_ID;
        actions[A_193_CUSTOM_IDLE_3] = A_193_CUSTOM_IDLE_3_ID;
        actions[A_194_CUSTOM_IDLE_4] = A_194_CUSTOM_IDLE_4_ID;
        actions[A_195_CUSTOM_IDLE_5] = A_195_CUSTOM_IDLE_5_ID;
        actions[A_196_CUSTOM_IDLE_6] = A_196_CUSTOM_IDLE_6_ID;
        actions[A_197_CUSTOM_IDLE_7] = A_197_CUSTOM_IDLE_7_ID;
        actions[A_198_CUSTOM_IDLE_8] = A_198_CUSTOM_IDLE_8_ID;
        actions[A_199_CUSTOM_IDLE_9] = A_199_CUSTOM_IDLE_9_ID;
        actions[A_200_CUSTOM_IDLE_10] = A_200_CUSTOM_IDLE_10_ID;
        actions[A_201_CUSTOM_WALK_1] = A_201_CUSTOM_WALK_1_ID;
        actions[A_202_CUSTOM_WALK_2] = A_202_CUSTOM_WALK_2_ID;
        actions[A_203_CUSTOM_WALK_3] = A_203_CUSTOM_WALK_3_ID;
        actions[A_204_CUSTOM_WALK_4] = A_204_CUSTOM_WALK_4_ID;
        actions[A_205_CUSTOM_WALK_5] = A_205_CUSTOM_WALK_5_ID;
        actions[A_206_CUSTOM_WALK_6] = A_206_CUSTOM_WALK_6_ID;
        actions[A_207_CUSTOM_WALK_7] = A_207_CUSTOM_WALK_7_ID;
        actions[A_208_CUSTOM_WALK_8] = A_208_CUSTOM_WALK_8_ID;
        actions[A_209_CUSTOM_WALK_9] = A_209_CUSTOM_WALK_9_ID;
        actions[A_210_CUSTOM_WALK_10] = A_210_CUSTOM_WALK_10_ID;
        actions[A_211_CUSTOM_RUN_1] = A_211_CUSTOM_RUN_1_ID;
        actions[A_212_CUSTOM_RUN_2] = A_212_CUSTOM_RUN_2_ID;
        actions[A_213_CUSTOM_RUN_3] = A_213_CUSTOM_RUN_3_ID;
        actions[A_214_CUSTOM_RUN_4] = A_214_CUSTOM_RUN_4_ID;
        actions[A_215_CUSTOM_RUN_5] = A_215_CUSTOM_RUN_5_ID;
        actions[A_216_CUSTOM_RUN_6] = A_216_CUSTOM_RUN_6_ID;
        actions[A_217_CUSTOM_RUN_7] = A_217_CUSTOM_RUN_7_ID;
        actions[A_218_CUSTOM_RUN_8] = A_218_CUSTOM_RUN_8_ID;
        actions[A_219_CUSTOM_RUN_9] = A_219_CUSTOM_RUN_9_ID;
        actions[A_220_CUSTOM_RUN_10] = A_220_CUSTOM_RUN_10_ID;
        actions[A_221_CUSTOM_JUMP_1] = A_221_CUSTOM_JUMP_1_ID;
        actions[A_222_CUSTOM_JUMP_2] = A_222_CUSTOM_JUMP_2_ID;
        actions[A_223_CUSTOM_JUMP_3] = A_223_CUSTOM_JUMP_3_ID;
        actions[A_224_CUSTOM_JUMP_4] = A_224_CUSTOM_JUMP_4_ID;
        actions[A_225_CUSTOM_JUMP_5] = A_225_CUSTOM_JUMP_5_ID;
        actions[A_226_CUSTOM_JUMP_6] = A_226_CUSTOM_JUMP_6_ID;
        actions[A_227_CUSTOM_JUMP_7] = A_227_CUSTOM_JUMP_7_ID;
        actions[A_228_CUSTOM_JUMP_8] = A_228_CUSTOM_JUMP_8_ID;
        actions[A_229_CUSTOM_JUMP_9] = A_229_CUSTOM_JUMP_9_ID;
        actions[A_230_CUSTOM_JUMP_10] = A_230_CUSTOM_JUMP_10_ID;
        actions[A_231_CUSTOM_JUMP_SPIN_1] = A_231_CUSTOM_JUMP_SPIN_1_ID;
        actions[A_232_CUSTOM_JUMP_SPIN_2] = A_232_CUSTOM_JUMP_SPIN_2_ID;
        actions[A_233_CUSTOM_JUMP_SPIN_3] = A_233_CUSTOM_JUMP_SPIN_3_ID;
        actions[A_234_CUSTOM_JUMP_SPIN_4] = A_234_CUSTOM_JUMP_SPIN_4_ID;
        actions[A_235_CUSTOM_JUMP_SPIN_5] = A_235_CUSTOM_JUMP_SPIN_5_ID;
        actions[A_236_CUSTOM_JUMP_SPIN_6] = A_236_CUSTOM_JUMP_SPIN_6_ID;
        actions[A_237_CUSTOM_JUMP_SPIN_7] = A_237_CUSTOM_JUMP_SPIN_7_ID;
        actions[A_238_CUSTOM_JUMP_SPIN_8] = A_238_CUSTOM_JUMP_SPIN_8_ID;
        actions[A_239_CUSTOM_JUMP_SPIN_9] = A_239_CUSTOM_JUMP_SPIN_9_ID;
        actions[A_240_CUSTOM_JUMP_SPIN_10] = A_240_CUSTOM_JUMP_SPIN_10_ID;
        actions[A_241_CUSTOM_FALL_1] = A_241_CUSTOM_FALL_1_ID;
        actions[A_242_CUSTOM_FALL_2] = A_242_CUSTOM_FALL_2_ID;
        actions[A_243_CUSTOM_FALL_3] = A_243_CUSTOM_FALL_3_ID;
        actions[A_244_CUSTOM_FALL_4] = A_244_CUSTOM_FALL_4_ID;
        actions[A_245_CUSTOM_FALL_5] = A_245_CUSTOM_FALL_5_ID;
        actions[A_246_CUSTOM_FALL_6] = A_246_CUSTOM_FALL_6_ID;
        actions[A_247_CUSTOM_FALL_7] = A_247_CUSTOM_FALL_7_ID;
        actions[A_248_CUSTOM_FALL_8] = A_248_CUSTOM_FALL_8_ID;
        actions[A_249_CUSTOM_FALL_9] = A_249_CUSTOM_FALL_9_ID;
        actions[A_250_CUSTOM_FALL_10] = A_250_CUSTOM_FALL_10_ID;
        actions[A_251_CUSTOM_LAND_1] = A_251_CUSTOM_LAND_1_ID;
        actions[A_252_CUSTOM_LAND_2] = A_252_CUSTOM_LAND_2_ID;
        actions[A_253_CUSTOM_LAND_3] = A_253_CUSTOM_LAND_3_ID;
        actions[A_254_CUSTOM_LAND_4] = A_254_CUSTOM_LAND_4_ID;
        actions[A_255_CUSTOM_LAND_5] = A_255_CUSTOM_LAND_5_ID;
        actions[A_256_CUSTOM_LAND_6] = A_256_CUSTOM_LAND_6_ID;
        actions[A_257_CUSTOM_LAND_7] = A_257_CUSTOM_LAND_7_ID;
        actions[A_258_CUSTOM_LAND_8] = A_258_CUSTOM_LAND_8_ID;
        actions[A_259_CUSTOM_LAND_9] = A_259_CUSTOM_LAND_9_ID;
        actions[A_260_CUSTOM_LAND_10] = A_260_CUSTOM_LAND_10_ID;
        actions[A_261_CUSTOM_PUNCH_1] = A_261_CUSTOM_PUNCH_1_ID;
        actions[A_262_CUSTOM_PUNCH_2] = A_262_CUSTOM_PUNCH_2_ID;
        actions[A_263_CUSTOM_PUNCH_3] = A_263_CUSTOM_PUNCH_3_ID;
        actions[A_264_CUSTOM_PUNCH_4] = A_264_CUSTOM_PUNCH_4_ID;
        actions[A_265_CUSTOM_PUNCH_5] = A_265_CUSTOM_PUNCH_5_ID;
        actions[A_266_CUSTOM_PUNCH_6] = A_266_CUSTOM_PUNCH_6_ID;
        actions[A_267_CUSTOM_PUNCH_7] = A_267_CUSTOM_PUNCH_7_ID;
        actions[A_268_CUSTOM_PUNCH_8] = A_268_CUSTOM_PUNCH_8_ID;
        actions[A_269_CUSTOM_PUNCH_9] = A_269_CUSTOM_PUNCH_9_ID;
        actions[A_270_CUSTOM_PUNCH_10] = A_270_CUSTOM_PUNCH_10_ID;
        actions[A_271_CUSTOM_KICK_1] = A_271_CUSTOM_KICK_1_ID;
        actions[A_272_CUSTOM_KICK_2] = A_272_CUSTOM_KICK_2_ID;
        actions[A_273_CUSTOM_KICK_3] = A_273_CUSTOM_KICK_3_ID;
        actions[A_274_CUSTOM_KICK_4] = A_274_CUSTOM_KICK_4_ID;
        actions[A_275_CUSTOM_KICK_5] = A_275_CUSTOM_KICK_5_ID;
        actions[A_276_CUSTOM_KICK_6] = A_276_CUSTOM_KICK_6_ID;
        actions[A_277_CUSTOM_KICK_7] = A_277_CUSTOM_KICK_7_ID;
        actions[A_278_CUSTOM_KICK_8] = A_278_CUSTOM_KICK_8_ID;
        actions[A_279_CUSTOM_KICK_9] = A_279_CUSTOM_KICK_9_ID;
        actions[A_280_CUSTOM_KICK_10] = A_280_CUSTOM_KICK_10_ID;
        actions[A_281_CUSTOM_ATTACK_1] = A_281_CUSTOM_ATTACK_1_ID;
        actions[A_282_CUSTOM_ATTACK_2] = A_282_CUSTOM_ATTACK_2_ID;
        actions[A_283_CUSTOM_ATTACK_3] = A_283_CUSTOM_ATTACK_3_ID;
        actions[A_284_CUSTOM_ATTACK_4] = A_284_CUSTOM_ATTACK_4_ID;
        actions[A_285_CUSTOM_ATTACK_5] = A_285_CUSTOM_ATTACK_5_ID;
        actions[A_286_CUSTOM_ATTACK_6] = A_286_CUSTOM_ATTACK_6_ID;
        actions[A_287_CUSTOM_ATTACK_7] = A_287_CUSTOM_ATTACK_7_ID;
        actions[A_288_CUSTOM_ATTACK_8] = A_288_CUSTOM_ATTACK_8_ID;
        actions[A_289_CUSTOM_ATTACK_9] = A_289_CUSTOM_ATTACK_9_ID;
        actions[A_290_CUSTOM_ATTACK_10] = A_290_CUSTOM_ATTACK_10_ID;
        actions[A_291_CUSTOM_MAGIC_1] = A_291_CUSTOM_MAGIC_1_ID;
        actions[A_292_CUSTOM_MAGIC_2] = A_292_CUSTOM_MAGIC_2_ID;
        actions[A_293_CUSTOM_MAGIC_3] = A_293_CUSTOM_MAGIC_3_ID;
        actions[A_294_CUSTOM_MAGIC_4] = A_294_CUSTOM_MAGIC_4_ID;
        actions[A_295_CUSTOM_MAGIC_5] = A_295_CUSTOM_MAGIC_5_ID;
        actions[A_296_CUSTOM_MAGIC_6] = A_296_CUSTOM_MAGIC_6_ID;
        actions[A_297_CUSTOM_MAGIC_7] = A_297_CUSTOM_MAGIC_7_ID;
        actions[A_298_CUSTOM_MAGIC_8] = A_298_CUSTOM_MAGIC_8_ID;
        actions[A_299_CUSTOM_MAGIC_9] = A_299_CUSTOM_MAGIC_9_ID;
        actions[A_300_CUSTOM_MAGIC_10] = A_300_CUSTOM_MAGIC_10_ID;
        actions[A_301_CUSTOM_BLOCK_1] = A_301_CUSTOM_BLOCK_1_ID;
        actions[A_302_CUSTOM_BLOCK_2] = A_302_CUSTOM_BLOCK_2_ID;
        actions[A_303_CUSTOM_BLOCK_3] = A_303_CUSTOM_BLOCK_3_ID;
        actions[A_304_CUSTOM_BLOCK_4] = A_304_CUSTOM_BLOCK_4_ID;
        actions[A_305_CUSTOM_BLOCK_5] = A_305_CUSTOM_BLOCK_5_ID;
        actions[A_306_CUSTOM_BLOCK_6] = A_306_CUSTOM_BLOCK_6_ID;
        actions[A_307_CUSTOM_BLOCK_7] = A_307_CUSTOM_BLOCK_7_ID;
        actions[A_308_CUSTOM_BLOCK_8] = A_308_CUSTOM_BLOCK_8_ID;
        actions[A_309_CUSTOM_BLOCK_9] = A_309_CUSTOM_BLOCK_9_ID;
        actions[A_310_CUSTOM_BLOCK_10] = A_310_CUSTOM_BLOCK_10_ID;
        actions[A_311_CUSTOM_HIT_1] = A_311_CUSTOM_HIT_1_ID;
        actions[A_312_CUSTOM_HIT_2] = A_312_CUSTOM_HIT_2_ID;
        actions[A_313_CUSTOM_HIT_3] = A_313_CUSTOM_HIT_3_ID;
        actions[A_314_CUSTOM_HIT_4] = A_314_CUSTOM_HIT_4_ID;
        actions[A_315_CUSTOM_HIT_5] = A_315_CUSTOM_HIT_5_ID;
        actions[A_316_CUSTOM_HIT_6] = A_316_CUSTOM_HIT_6_ID;
        actions[A_317_CUSTOM_HIT_7] = A_317_CUSTOM_HIT_7_ID;
        actions[A_318_CUSTOM_HIT_8] = A_318_CUSTOM_HIT_8_ID;
        actions[A_319_CUSTOM_HIT_9] = A_319_CUSTOM_HIT_9_ID;
        actions[A_320_CUSTOM_HIT_10] = A_320_CUSTOM_HIT_10_ID;
        actions[A_321_CUSTOM_LOSE_1] = A_321_CUSTOM_LOSE_1_ID;
        actions[A_322_CUSTOM_LOSE_2] = A_322_CUSTOM_LOSE_2_ID;
        actions[A_323_CUSTOM_LOSE_3] = A_323_CUSTOM_LOSE_3_ID;
        actions[A_324_CUSTOM_LOSE_4] = A_324_CUSTOM_LOSE_4_ID;
        actions[A_325_CUSTOM_LOSE_5] = A_325_CUSTOM_LOSE_5_ID;
        actions[A_326_CUSTOM_LOSE_6] = A_326_CUSTOM_LOSE_6_ID;
        actions[A_327_CUSTOM_LOSE_7] = A_327_CUSTOM_LOSE_7_ID;
        actions[A_328_CUSTOM_LOSE_8] = A_328_CUSTOM_LOSE_8_ID;
        actions[A_329_CUSTOM_LOSE_9] = A_329_CUSTOM_LOSE_9_ID;
        actions[A_330_CUSTOM_LOSE_10] = A_330_CUSTOM_LOSE_10_ID;
        actions[A_331_CUSTOM_DIE_1] = A_331_CUSTOM_DIE_1_ID;
        actions[A_332_CUSTOM_DIE_2] = A_332_CUSTOM_DIE_2_ID;
        actions[A_333_CUSTOM_DIE_3] = A_333_CUSTOM_DIE_3_ID;
        actions[A_334_CUSTOM_DIE_4] = A_334_CUSTOM_DIE_4_ID;
        actions[A_335_CUSTOM_DIE_5] = A_335_CUSTOM_DIE_5_ID;
        actions[A_336_CUSTOM_DIE_6] = A_336_CUSTOM_DIE_6_ID;
        actions[A_337_CUSTOM_DIE_7] = A_337_CUSTOM_DIE_7_ID;
        actions[A_338_CUSTOM_DIE_8] = A_338_CUSTOM_DIE_8_ID;
        actions[A_339_CUSTOM_DIE_9] = A_339_CUSTOM_DIE_9_ID;
        actions[A_340_CUSTOM_DIE_10] = A_340_CUSTOM_DIE_10_ID;
        actions[A_341_CUSTOM_VICTORY_1] = A_341_CUSTOM_VICTORY_1_ID;
        actions[A_342_CUSTOM_VICTORY_2] = A_342_CUSTOM_VICTORY_2_ID;
        actions[A_343_CUSTOM_VICTORY_3] = A_343_CUSTOM_VICTORY_3_ID;
        actions[A_344_CUSTOM_VICTORY_4] = A_344_CUSTOM_VICTORY_4_ID;
        actions[A_345_CUSTOM_VICTORY_5] = A_345_CUSTOM_VICTORY_5_ID;
        actions[A_346_CUSTOM_VICTORY_6] = A_346_CUSTOM_VICTORY_6_ID;
        actions[A_347_CUSTOM_VICTORY_7] = A_347_CUSTOM_VICTORY_7_ID;
        actions[A_348_CUSTOM_VICTORY_8] = A_348_CUSTOM_VICTORY_8_ID;
        actions[A_349_CUSTOM_VICTORY_9] = A_349_CUSTOM_VICTORY_9_ID;
        actions[A_350_CUSTOM_VICTORY_10] = A_350_CUSTOM_VICTORY_10_ID;
        actions[A_351_CUSTOM_EXTRA_ACTIONS_1] = A_351_CUSTOM_EXTRA_ACTIONS_1_ID;
        actions[A_352_CUSTOM_EXTRA_ACTIONS_2] = A_352_CUSTOM_EXTRA_ACTIONS_2_ID;
        actions[A_353_CUSTOM_EXTRA_ACTIONS_3] = A_353_CUSTOM_EXTRA_ACTIONS_3_ID;
        actions[A_354_CUSTOM_EXTRA_ACTIONS_4] = A_354_CUSTOM_EXTRA_ACTIONS_4_ID;
        actions[A_355_CUSTOM_EXTRA_ACTIONS_5] = A_355_CUSTOM_EXTRA_ACTIONS_5_ID;
        actions[A_356_CUSTOM_EXTRA_ACTIONS_6] = A_356_CUSTOM_EXTRA_ACTIONS_6_ID;
        actions[A_357_CUSTOM_EXTRA_ACTIONS_7] = A_357_CUSTOM_EXTRA_ACTIONS_7_ID;
        actions[A_358_CUSTOM_EXTRA_ACTIONS_8] = A_358_CUSTOM_EXTRA_ACTIONS_8_ID;
        actions[A_359_CUSTOM_EXTRA_ACTIONS_9] = A_359_CUSTOM_EXTRA_ACTIONS_9_ID;
        actions[A_360_CUSTOM_EXTRA_ACTIONS_10] = A_360_CUSTOM_EXTRA_ACTIONS_10_ID;
        backActionName = A_011_IDLE_1;
        backActionID = A_011_IDLE_1_ID;
        UpdateAnimationAction();

        // get a reference to our main camera
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

    }

    private void Start()
    {
        UpdateAnimationAction();
    }

    private void Update()
    {
        UpdateAnimationAction();
    }

    public void FindComponents()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        controller = GetComponent<CharacterController>();
    }

    public void Jump()
    {
        if (isGrounded == true)
        {
            rigidbody.velocity = new Vector3(0f, jumpForce, 0f);
            isGrounded = false;
            isJumping = true;
            EnableDoubleJumping = true;
            isFalling = false;
        }
        else
        {
            if (EnableDoubleJumping == true)
            {
                if ((isJumping == true) && isDoubleJumping == false)
                {
                    rigidbody.velocity = new Vector3(0f, jumpForce * 1.5f, 0f);
                    isDoubleJumping = true;
                    isFalling = false;
                }
            }
        }
    }

    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = input.sprint ? SprintSpeed : MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (input.move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (input.move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        // move the player
        controller.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                         new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator if using character
/*        if (animator != null)
        {
            animator.SetFloat(animIDSpeed, animationBlend);
            animator.SetFloat(animIDMotionSpeed, inputMagnitude);
        }*/
    }

    public Vector3 GetRigidBoy_Velocity()
    {
        return rigidbody.velocity;
    }

    public void ActionNoLoopedReturnToIdle(bool value)
    {
        actionNoLoopedReturnToIdle = value;
    }

    public void SetActionInt(int _actionID = -1)
    {
        ActionNoLoopedReturnToIdle(true);
        if (_actionID == 61)
        {
            gameObject.transform.position = new Vector3(0, 2.5f, 0);
        }
        StopCoroutine("ReturnToActionCoroutine");
        actionID = _actionID;
        FindComponents();
        animator.SetInteger("actionID", actionID);
        StopAllCoroutines();
        try
        {
            float waitTime = 1.0f;
            if (animator != null)
            {
                animatorInfo = this.animator.GetCurrentAnimatorClipInfo(0);
                for (int i = 0; i < animatorInfo.Length; i++)
                {
                }
                if (animatorInfo.Length > 0)
                {
                    currentAnimationClip = animatorInfo[0].clip;
                    if (currentAnimationClip != null)
                    {
                    }
                    currentAnimation = currentAnimationClip.name;
                    float clipDuration = currentAnimationClip.length;
                    float animatorSpeed = this.animator.speed;
                    waitTime = clipDuration / animatorSpeed;
                }
            }
            StartCoroutine("WaitToActionCoroutine", waitTime);
        }
        catch (IndexOutOfRangeException e)
        {
            throw new ArgumentOutOfRangeException("index parameter is out of range.", e);
        }
        finally
        {
        }
        UpdateAnimationAction();
    }

    public void SetActionName(string _actionName = "011_idle_1")
    {
        StopCoroutine("ReturnToActionCoroutine");
        actionID = (int)actions[_actionName];
        animator.SetInteger("actionID", actionID);
        UpdateAnimationAction();
    }

    public void SetAnimatorSpeed(float _speed = 1)
    {
        animator.speed = _speed;
    }

    private void UpdateAnimationAction()
    {
        if (rigidbody == null)
        {
            Debug.LogWarning("rigidbody NOT FOUND!");
            return;
        }
        if (rigidbody.velocity.y > .1f)
        {
            if (actionID != (int)actions[A_041_JUMP_1] && isDoubleJumping == false)
            {
                actionID = (int)actions[A_041_JUMP_1];
                SetActionInt(41);
            }
            if (actionID != (int)actions[A_051_JUMP_SPIN_1] && isDoubleJumping == true)
            {
                actionID = (int)actions[A_051_JUMP_SPIN_1];
                SetActionInt(51);
            }
        }
        else if (rigidbody.velocity.y < -.1f && isFalling == false)
        {
            if (actionID != (int)actions[A_061_FALL_1])
            {
                SetActionInt(61);
                isJumping = false;
                isFalling = true;
                isDoubleJumping = false;
            }
        }
        else if (rigidbody.velocity.y == 0)
        {
            if (actionID != (int)actions[A_071_LAND_1] && actionID == (int)actions[A_061_FALL_1])
            {
                SetActionInt(71);
                isJumping = false;
                isFalling = false;
                isDoubleJumping = false;
            }
        }
        if (transform.position.y <= -10)
        {
            //RestarLevel();
            Respawn();
        }
    }

    private void ReturnToAction(string _actionName = "011_idle_1", float _returnTime = 2.0f)
    {
        backActionName = _actionName;
        backActionID = (int)actions[_actionName];
        StopCoroutine("ReturnToActionCoroutine");
        StartCoroutine("ReturnToActionCoroutine", _returnTime);
    }

    private IEnumerator ReturnToActionCoroutine(float _returnTime = 3.0f)
    {
        yield return new WaitForSeconds(_returnTime);
        if (actionNoLoopedReturnToIdle == true)
        {
            if (backActionID != -1)
            {
                SetActionInt(backActionID);
            }
            else if (backActionName != "")
            {
                SetActionName(backActionName);
            }
        }
    }

    private IEnumerator WaitToActionCoroutine(float _returnTime = 2.0f)
    {
        yield return new WaitForSeconds(_returnTime);
        try
        {
            bool hasLoop = true;
            float clipDuration = 1;
            float animatorSpeed = 1;
            if (animator != null)
            {
                animatorInfo = this.animator.GetCurrentAnimatorClipInfo(0);
                for (int i = 0; i < animatorInfo.Length; i++)
                {
                }
                if (animatorInfo.Length > 0)
                {
                    currentAnimationClip = animatorInfo[0].clip;
                    currentAnimation = currentAnimationClip.name;
                    clipDuration = currentAnimationClip.length;
                    animatorSpeed = this.animator.speed;
                    hasLoop = currentAnimationClip.isLooping;
                }
            }
            if (hasLoop == true)
            {
            }
            else
            {
                ReturnToAction(backActionName, clipDuration * animatorSpeed);
            }
        }
        catch (IndexOutOfRangeException e)
        {
            throw new ArgumentOutOfRangeException("index parameter is out of range.", e);
        }
        finally
        {
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
            if (actionID == (int)actions[A_071_LAND_1])
            {
                SetActionName(A_071_LAND_1);
                isFalling = false;
                isJumping = false;
                isDoubleJumping = false;
                EnableDoubleJumping = false;
                ReturnToAction(A_011_IDLE_1, 1.0f);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void RestarLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Respawn()
    {
        gameObject.transform.position = new Vector3(0, 5, 0);
    }
}